using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using static Android.Views.View;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Firebase.Auth;
using Firebase.Database;
using Java.Util;
using XamarinChat.Adapters;
using XamarinChat.Models;

namespace XamarinChat
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class MessageActivity : AppCompatActivity, IOnClickListener, IValueEventListener
    {
        private FirebaseClient firebaseClient;
        private List<ChatModel> chatsList = new List<ChatModel>();
        private RecyclerView recyclerView;
        private Button button_send;
        private EditText text_send;

        private string receiverID;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.chat_layout);
            
            recyclerView = (RecyclerView)FindViewById(Resource.Id.recycler_view);

            receiverID = Intent.GetStringExtra("ID");
            
            firebaseClient = new FirebaseClient("Paste here your firebase_url");
            FirebaseDatabase.Instance.GetReference("Chats").AddValueEventListener(this);

            button_send = FindViewById<Button>(Resource.Id.send_message);
            text_send = FindViewById<EditText>(Resource.Id.input_message);
            button_send.SetOnClickListener(this);
            
            DisplayChatMessage();
        }
        
        public void OnCancelled(DatabaseError error)
        {
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            DisplayChatMessage();
        }
        
        private void SetupRecyclerView()
        {
            recyclerView.SetLayoutManager(new LinearLayoutManager(recyclerView.Context));
            ChatAdapter adapter = new ChatAdapter(chatsList);
            recyclerView.SetAdapter(adapter);
        }
        
        private async void DisplayChatMessage()
        {
            chatsList.Clear();
            var items = await firebaseClient.Child("Chats").OnceAsync<ChatModel>();
            foreach (var item in items)
            {
                ChatModel chat = new ChatModel();
                chat.SenderID = item.Object.SenderID;
                chat.ReceiverID = item.Object.ReceiverID;

                if (chat.ReceiverID.Equals(FirebaseAuth.Instance.CurrentUser.Uid) && chat.SenderID.Equals(receiverID) ||
                    chat.ReceiverID.Equals(receiverID) && chat.SenderID.Equals(FirebaseAuth.Instance.CurrentUser.Uid))
                {
                    chatsList.Add(item.Object);
                }
                SetupRecyclerView();
            }
        }

        private void SendMessage(string sender, string receiver, string message)
        {
            HashMap hashMap = new HashMap();
            hashMap.Put("SenderID", sender);
            hashMap.Put("ReceiverID", receiver);
            hashMap.Put("Message", message);
            FirebaseDatabase.Instance.Reference.Child("Chats").Push().SetValue(hashMap);
        }

        public void OnClick(View view)
        {
            if (view.Id == Resource.Id.send_message)
            {
                SendMessage(FirebaseAuth.Instance.CurrentUser.Uid, receiverID, text_send.Text);
                text_send.Text = "";
            }
        }
    }
}
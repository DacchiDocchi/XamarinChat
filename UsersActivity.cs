using Android.App;
using Android.OS;
using System.Collections.Generic;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Firebase.Database;
using XamarinChat.Adapters;
using XamarinChat.Models;

namespace XamarinChat
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class UsersActivity : AppCompatActivity, IValueEventListener
    {
        private RecyclerView recyclerView;
        private List<UserModel> usersList = new List<UserModel>();
        private FirebaseClient firebaseClient;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.users_layout);
            recyclerView = (RecyclerView) FindViewById(Resource.Id.myRecyclerView);
            firebaseClient = new FirebaseClient("Paste here your firebase_url");
            FirebaseDatabase.Instance.GetReference("Users").AddValueEventListener(this);
            DisplayUsers();
        }
        
        private void SetupRecyClerView()
        {
            recyclerView.SetLayoutManager(new LinearLayoutManager(recyclerView.Context));
            UsersAdapter adapter = new UsersAdapter(usersList);
            recyclerView.SetAdapter(adapter);
        }

        private async void DisplayUsers()
        {
            var items = await firebaseClient.Child("Users").OnceAsync<UserModel>();
            usersList.Clear();
            foreach (var item in items)
            {
                UserModel user = new UserModel();
                user.ID = item.Object.ID;
                user.Email = item.Object.Email;
                usersList.Add(item.Object);
            }

            SetupRecyClerView();
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            DisplayUsers();
        }

        public void OnCancelled(DatabaseError error)
        {
        }
    }
}
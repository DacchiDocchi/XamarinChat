using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using AndroidX.RecyclerView.Widget;
using Firebase.Auth;
using XamarinChat.Models;

namespace XamarinChat.Adapters
{
    class ChatAdapter : RecyclerView.Adapter
    {
        public static int MSG_TYPE_RIGHT = 1;
        public static int MSG_TYPE_LEFT = 0;
        
        List<ChatModel> Items;

        public ChatAdapter(List<ChatModel>Data)
        {
            Items = Data;
        }
        
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (viewType == MSG_TYPE_RIGHT) {
                View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.chat_item_right, parent, false);
                return new ChatAdapterViewHolder(itemView);
            } else {
                View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.chat_item_left, parent, false);
                return new ChatAdapterViewHolder(itemView);
            }
        }
        
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var holder = viewHolder as ChatAdapterViewHolder;
            holder.show_message.Text = Items[position].Message;
        }

        public override int ItemCount => Items.Count;
        
        public override int GetItemViewType(int position)
        {
            if (Items[position].SenderID.Equals(FirebaseAuth.Instance.CurrentUser.Uid)){ 
                return MSG_TYPE_RIGHT;
            } else {
                return MSG_TYPE_LEFT;
            }
        }
    }

    public class ChatAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView show_message { get; set; }
        public ChatAdapterViewHolder(View itemView) : base(itemView)
        {
            show_message = (TextView)itemView.FindViewById(Resource.Id.show_message);
        }
    }
}
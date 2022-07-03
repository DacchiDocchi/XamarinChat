using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using Android.Content;
using AndroidX.RecyclerView.Widget;
using XamarinChat.Models;

namespace XamarinChat.Adapters
{
    class UsersAdapter : RecyclerView.Adapter
    {
        List<UserModel> Items;

        public UsersAdapter(List<UserModel>Data)
        {
            Items = Data;
        }
        
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.users_row, parent, false);
           
            var vh = new UsersAdapterViewHolder(itemView);
            return vh;
        }
        
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var holder = viewHolder as UsersAdapterViewHolder;
            holder.emailText.Text = Items[position].Email;

            holder.emailText.Click += delegate
            {
                Intent intent = new Intent(holder.emailText.Context, typeof(MessageActivity));
                intent.PutExtra("ID", Items[position].ID);
                holder.emailText.Context.StartActivity(intent);
            };
        }

        public override int ItemCount => Items.Count;
    }

    public class UsersAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView emailText { get; set; }
        public UsersAdapterViewHolder(View itemView) : base(itemView)
        {
            emailText = (TextView)itemView.FindViewById(Resource.Id.nameText);
        }
    }
}
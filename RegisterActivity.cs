using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Firebase.Auth;
using Firebase.Database;
using Java.Util;

namespace XamarinChat
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", NoHistory = true)]
    public class RegisterActivity : AppCompatActivity, View.IOnClickListener, IOnCompleteListener
    {
        private Button register_button;
        private EditText input_email, input_password;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.register_layout);

            input_email = FindViewById<EditText>(Resource.Id.input_register_email);
            input_password = FindViewById<EditText>(Resource.Id.input_register_password);
            
            register_button = FindViewById<Button>(Resource.Id.register_button);
            register_button.SetOnClickListener(this);
        }

        private void RegisterAccount(string email, string password)
        {
            FirebaseAuth.Instance.CreateUserWithEmailAndPassword(email, password).AddOnCompleteListener(this);
        }
        
        public void OnClick(View view)
        {
            if (view.Id == Resource.Id.register_button)
            {
                RegisterAccount(input_email.Text, input_password.Text);
            }
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                HashMap hashMap = new HashMap();
                hashMap.Put("ID", FirebaseAuth.Instance.CurrentUser.Uid);
                hashMap.Put("Email", FirebaseAuth.Instance.CurrentUser.Email);
                FirebaseDatabase.Instance.Reference.Child("Users").Push().SetValue(hashMap);
                
                Intent intent = new Intent(this, typeof(UsersActivity));
                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this, "Register failed, try again!", ToastLength.Short).Show();
            }
        }
    }
}
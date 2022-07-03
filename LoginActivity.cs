using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Firebase.Auth;

namespace XamarinChat
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", NoHistory = true)]
    public class LoginActivity : AppCompatActivity, View.IOnClickListener, IOnCompleteListener
    {
        private Button button_login;
        private EditText input_email, input_password;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login_layout);

            input_email = FindViewById<EditText>(Resource.Id.input_login_email);
            input_password = FindViewById<EditText>(Resource.Id.input_login_password);
            
            button_login = FindViewById<Button>(Resource.Id.login_button);
            button_login.SetOnClickListener(this);
        }

        private void LoginAccount(string email, string password)
        {
            FirebaseAuth.Instance.SignInWithEmailAndPassword(email, password).AddOnCompleteListener(this);
        }
        
        public void OnClick(View view)
        {
            if (view.Id == Resource.Id.login_button)
            {
                LoginAccount(input_email.Text, input_password.Text);
            }
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                Intent intent = new Intent(this, typeof(UsersActivity));
                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this, "Login failed, try again!", ToastLength.Short).Show();
            }
        }
    }
}
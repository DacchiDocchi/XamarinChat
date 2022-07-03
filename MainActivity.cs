using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Firebase.Auth;

namespace XamarinChat
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, View.IOnClickListener
    {
        private Button register_button, login_button;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.start_layout);

            login_button = FindViewById<Button>(Resource.Id.start_login_button);
            register_button = FindViewById<Button>(Resource.Id.start_register_button);
            
            login_button.SetOnClickListener(this);
            register_button.SetOnClickListener(this);
        }
        
        protected override void OnStart()
        {
            base.OnStart();
            FirebaseUser firebaseUser = FirebaseAuth.Instance.CurrentUser;

            if (firebaseUser != null)
            {
                Intent intent = new Intent(this, typeof(UsersActivity));
                StartActivity(intent);
                Finish();
            }
        }

        public void OnClick(View view)
        {
            if (view.Id == Resource.Id.start_login_button)
            {
                StartActivity(new Intent(this, typeof(LoginActivity)));
            }

            if (view.Id == Resource.Id.start_register_button)
            {
                StartActivity(new Intent(this, typeof(RegisterActivity)));
            }
        }
    }
}
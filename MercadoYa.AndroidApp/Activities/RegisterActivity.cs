using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Widget;
//using Firebase.Auth;
using MercadoYa.AndroidApp.Handlers_nd_Helpers;
using MercadoYa.AndroidApp.HandlersAndServices;
using MercadoYa.Interfaces;
using MercadoYa.Model.Concrete;
using System;
using System.Threading.Tasks;

namespace MercadoYa.AndroidApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/MercadoYa.Theme", MainLauncher = false)]
    public class RegisterActivity : AppCompatActivity
    {
        TextInputLayout txtName;
        TextInputLayout txtPhone;
        TextInputLayout txtEmail;
        TextInputLayout txtPassword;
        Button btnRegister;
        IObservableClientAuthenticator Authenticator;
        CoordinatorLayout RootView;
        TextView txtClickToLogin;
        IFullAppUser Customer;
        AnimationHandler Animator;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.register);
            InitControls();
            ResolveDependencies();
        }

        void ResolveDependencies()
        {
            Authenticator = App.DiContainer.Resolve<IObservableClientAuthenticator>();
            Customer = new FullCustomerUser();
        }

        void InitControls()
        {
            this.RootView = (CoordinatorLayout)FindViewById(Resource.Id.rootView);
            this.txtName = (TextInputLayout)FindViewById(Resource.Id.txtFullName);
            this.txtPhone = (TextInputLayout)FindViewById(Resource.Id.txtPhoneNumber);
            this.txtEmail = (TextInputLayout)FindViewById(Resource.Id.txtEmail);
            this.txtPassword = (TextInputLayout)FindViewById(Resource.Id.txtPassword);
            this.btnRegister = (Button)FindViewById(Resource.Id.btnRegister);
            this.txtClickToLogin = (TextView)FindViewById(Resource.Id.txtGoToLogin);
            var ProgBar = (ProgressBar)FindViewById(Resource.Id.loadingProgress);
            this.Animator = new AnimationHandler(ProgBar);

            btnRegister.Click += BtnRegister_Click;
            txtClickToLogin.Click += TxtClickToLogin_Click;
        }

        private void TxtClickToLogin_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(LoginActivity));
            Finish();
        }
        async void BtnRegister_Click(object sender, EventArgs e)
        {
            Animator.FadeIn();

            Customer.Username = txtName.EditText.Text;
            Customer.Phone = txtPhone.EditText.Text;
            Customer.Email = txtEmail.EditText.Text;
            Customer.Password = txtPassword.EditText.Text;

            if (UserUtil.PromptIfInvalid(RootView, Customer))
                return;

            await RegisterUser();
        }
        async Task RegisterUser()
        {
            var TaskCompletionListener = new AuthCompletionListener(TaskCompletionListener_Success, TaskCompletionListener_Failure);
            Authenticator.AddOnFailureListener(TaskCompletionListener);
            Authenticator.AddOnSuccessListener(TaskCompletionListener);
            await Authenticator.RegisterUserAsync(Customer);
        }

        void TaskCompletionListener_Failure(object sender, Exception e)
        {
            Animator.FadeOut();
            Toaster.SomethingWentWrong(this);
        }

        void TaskCompletionListener_Success(object sender, IAuthResult Result)
        {
            Animator.FadeOut();
            Snackbar.Make(RootView, "¡Registro exitoso!", Snackbar.LengthShort).Show();
            var User = Result.User as FullCustomerUser;
            UserUtil.SaveIfValid(User.Email, User.Password);
            StartActivity(typeof(MainActivity));
            Finish();
        }

    }
}
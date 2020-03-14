using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Widget;
using MercadoYa.AndroidApp.Handlers_nd_Helpers;
using MercadoYa.AndroidApp.HandlersAndServices;
using MercadoYa.Interfaces;
using MercadoYa.Model.Concrete;
using System;

namespace MercadoYa.AndroidApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/MercadoYa.Theme", MainLauncher = false)]
    public class LoginActivity : AppCompatActivity
    {

        TextView txtGoToRegister;
        TextInputLayout txtEmail;
        TextInputLayout txtPassword;
        Button btnLogin;
        CoordinatorLayout RootView;
        IObservableClientAuthenticator Authenticator;
        AnimationHandler Animator;

        string Email, Password;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.login);
            InitControls();
            ResolveDependencies();
        }

        private void ResolveDependencies()
        {
            Authenticator = App.DiContainer.Resolve<IObservableClientAuthenticator>();
        }

        private void InitControls()
        {
            this.RootView = (CoordinatorLayout)FindViewById(Resource.Id.rootView);
            this.txtGoToRegister = (TextView)FindViewById(Resource.Id.txtGoToRegister);
            this.txtEmail = (TextInputLayout)FindViewById(Resource.Id.txtEmail);
            this.txtPassword = (TextInputLayout)FindViewById(Resource.Id.txtPassword);
            this.btnLogin = (Button)FindViewById(Resource.Id.btnLogin);
            var ProgBar = (ProgressBar)FindViewById(Resource.Id.loadingProgress);
            this.Animator = new AnimationHandler(ProgBar);

            btnLogin.Click += BtnLogin_Click;
            txtGoToRegister.Click += TxtGoToRegister_Click;

        }
        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            Animator.FadeIn();

            Email = txtEmail.EditText.Text;
            Password = txtPassword.EditText.Text;

            if (UserUtil.PromptIfInvalid(RootView, Email, Password))
                return;

            var TaskCompletionListener = new AuthCompletionListener(TaskCompletionListener_Success, TaskCompletionListener_Failure);

            Authenticator.AddOnFailureListener(TaskCompletionListener);
            Authenticator.AddOnSuccessListener(TaskCompletionListener);
            await Authenticator.SignInWithEmailAndPasswordAsync(Email, Password);
        }

        private void TaskCompletionListener_Failure(object sender, Exception e)
        {
            Animator.FadeOut();
            Toaster.SomethingWentWrong(this);
        }

        private void TaskCompletionListener_Success(object sender, IAuthResult Result)
        {
            Animator.FadeOut();
            var User = Result.User as FullCustomerUser;
            UserUtil.SaveIfValid(User.Email, User.Password);
            StartActivity(typeof(MainActivity));
            Finish();
        }

        private void TxtGoToRegister_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(RegisterActivity));
            Finish();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp14
{
    interface IChain
    {
        IChain NextChain { get; set; }
        List<User> Users { get; set; }
        void SetNextChain(IChain chain);
        void SignInOrUp(User user);
    }

    class User
    {
        public User(string username, string password, string signInOrUp)
        {
            Username = username;
            Password = password;
            SignInOrUp = signInOrUp;
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string SignInOrUp { get; set; }
    }

    class SignUp : IChain
    {

        public IChain NextChain { get; set; }
        public List<User> Users { get; set; } = new List<User>();
        public SignUp(List<User> users)
        {
            Users = users;
        }

        public void SetNextChain(IChain chain)
        {
            NextChain = chain;
        }

        public void SignInOrUp(User user)
        {
            if (user.SignInOrUp == "SignUp")
            {
                Users.Add(new User(user.Username, user.Password, "SignIn"));
                user.SignInOrUp = "SignIn";
                Console.WriteLine("Sign up is successfully done, now you directed Sign In process");
                NextChain.Users = Users;
                NextChain.SignInOrUp(user);
            }
            else
            {
                NextChain.Users = Users;
                NextChain.SignInOrUp(user);
            }
        }
    }
    class SignIn : IChain
    {

        public IChain NextChain { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        public void SetNextChain(IChain chain)
        {
            NextChain = chain;
        }

        public void SignInOrUp(User user)
        {
            if (user.SignInOrUp == "SignIn")
            {
                var User = Users.SingleOrDefault((i) => user.Username == i.Username && user.Password == i.Password);
                if (User != null)
                {
                    user.SignInOrUp = "Order";
                    NextChain.SignInOrUp(user);
                }
                else
                {
                    Console.WriteLine("End of chain!");
                }
            }
            else
            {
                NextChain.Users = Users;
                NextChain.SignInOrUp(user);
            }
        }
    }
    class Order : IChain
    {

        public IChain NextChain { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        public void SetNextChain(IChain chain)
        {
            throw new NotImplementedException();
        }

        public void SignInOrUp(User user)
        {
            if (user.SignInOrUp == "Order")
            {
                Console.WriteLine("Your order has been received successfully");
            }
            else
            {
                Console.WriteLine("This is end of chain");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<User> users = new List<User>
            {
                new User("admin","admin123","SignIn"),
                new User("guest","guest123","SignIn"),
                new User("sun","sun123","SignIn"),
            };
            SignUp s = new SignUp(users);
            SignIn s2 = new SignIn();
            s.SetNextChain(s2);
            s2.SetNextChain(new Order());
            s.SignInOrUp(new User("admin2", "admin246", "SignUp"));
        }
    }
}

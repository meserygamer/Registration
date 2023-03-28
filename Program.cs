namespace Registration
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите логин:");
            string login = Console.ReadLine();
            Console.WriteLine("Введите пароль:");
            string password = Console.ReadLine();
            Auth_data? User = Registration.Authorization(login, password);
            if (User is not null)
            {
                Console.WriteLine($"Добро пожаловать {((Auth_data)User).Familiya} {((Auth_data)User).Name}");
                if(((Auth_data)User).role == Roles.Admin)
                {
                    Console.WriteLine("Вам доступны следующие действия:");
                    Console.WriteLine(" 1 просмотр своей учетной записи\r\n\r\n" +
                        " 2 просмотр учетных записей всех пользователей\r\n\r\n" +
                        " 3 просмотр данных совершеннолетних пользователей\r\n\r\n" +
                        " 4 просмотр данных об определенном пользователе (по введенным имени и фамилии)\r\n\r\n" +
                        " 5 регистрация нового пользователя");
                    int chose = Convert.ToInt32(Console.ReadLine());
                    switch (chose)
                    {
                        case 1:((Auth_data)User).Prosm() ; break;
                        case 2:Registration.prosmAll() ; break;
                        case 3:Registration.prosmSov() ; break;
                        case 4:
                            {
                                Console.WriteLine("Введите имя и фамилию");
                                Registration.prosmFam(Console.ReadLine(), Console.ReadLine());
                                break;
                            }
                        case 5: Registration.RegSU(); break;
                    }
                }
                else
                {
                    Console.WriteLine("Желаете просмотреть свои данные? (да/нет)");
                    switch (Console.ReadLine())
                    {
                        case "да": ((Auth_data)User).Prosm(); break;
                        default: Console.WriteLine("Очень жаль"); break;
                    }
                }
            }
            else Console.WriteLine("Вас нет в системе");
        }
    }
    enum Roles
    { 
        Admin,
        SimpleUser
    }
    struct Auth_data
    {
        public int Id { get; private set; }
        public string Familiya { get; private set; } = "";
        public string Name { get; private set; } = "";
        public int age { get; private set; }
        public string login { get; private set; } = "";
        public string password { get; private set; } = "";
        public Roles role { get; private set; }
        public Auth_data(Roles role, int id)
        {
            Id = id;
            Console.WriteLine("Введите фамилию пользователя");
            Familiya = Console.ReadLine();
            Console.WriteLine("Введите имя пользователя");
            Name = Console.ReadLine();
            Console.WriteLine("Введите возраст пользователя");
            age = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите логин пользователя");
            login = Console.ReadLine();
            Console.WriteLine("Введите пароль пользователя");
            password = Console.ReadLine();
            this.role = role;
        }
        public Auth_data(int ID, string FAMILIYA, string NAME, int AGE, string LOGIN, string PASSWORD, int ROLE)
        {
            Id = ID;
            Familiya = FAMILIYA;
            Name = NAME;
            age = AGE;
            login = LOGIN;
            password = PASSWORD;
            role = (Roles)ROLE;
        }
        public void Prosm()
        {
            Console.WriteLine($"Id: {Id} Фамилия: {Familiya} Имя: {Name} Возраст: {age} Логин: {login} Пароль: {password}");
        }
    }
    class Registration
    {
        static Auth_data Person;
        public static void RegA()
        {
            Person = new Auth_data(Roles.Admin, NextID());
            BinaryWriter BW = new BinaryWriter(File.Open("Authdata.txt", FileMode.OpenOrCreate));
            BW.Seek(0, SeekOrigin.End);
            BW.Write(Person.Id);
            BW.Write(Person.Familiya);
            BW.Write(Person.Name);
            BW.Write(Person.age);
            BW.Write(Person.login);
            BW.Write(Person.password);
            BW.Write((int)Person.role);
            BW.Close();
        }
        public static void RegSU()
        {
            Person = new Auth_data(Roles.SimpleUser, NextID());
            BinaryWriter BW = new BinaryWriter(File.Open("Authdata.txt", FileMode.OpenOrCreate));
            BW.Seek(0, SeekOrigin.End);
            BW.Write(Person.Id);
            BW.Write(Person.Familiya);
            BW.Write(Person.Name);
            BW.Write(Person.age);
            BW.Write(Person.login);
            BW.Write(Person.password);
            BW.Write((int)Person.role);
            BW.Close();
        }
        private static int NextID()
        {
            Random r = new Random();
            int randch = r.Next(1000, 10000);
            BinaryReader BR = new BinaryReader(File.Open("Authdata.txt", FileMode.OpenOrCreate));
            if (BR.PeekChar() == -1)
            {
                BR.Close();
                return randch;
            }
            while (BR.PeekChar() != -1)
            {
                if (BR.ReadInt32() != randch)
                {
                    BR.Close();
                    return randch;
                }
                BR.ReadString();
                BR.ReadString();
                BR.ReadInt32();
                BR.ReadString();
                BR.ReadString();
                BR.ReadInt32();
            }
            BR.Close();
            return NextID();
        }
        public static Auth_data? Authorization(string login, string password)
        {
            BinaryReader BR = new BinaryReader(File.Open("Authdata.txt", FileMode.OpenOrCreate));
            while (BR.PeekChar() != -1)
            {
                Person = new Auth_data(BR.ReadInt32(), BR.ReadString(), BR.ReadString(), BR.ReadInt32(), BR.ReadString(), BR.ReadString(), BR.ReadInt32());
                if(Person.login == login && Person.password == password)
                {
                    BR.Close();
                    return Person;
                }
            }
            BR.Close();
            return null;
        }
        public static void prosmAll()
        {
            BinaryReader BR = new BinaryReader(File.Open("Authdata.txt", FileMode.OpenOrCreate));
            while (BR.PeekChar() != -1)
            {
                Person = new Auth_data(BR.ReadInt32(), BR.ReadString(), BR.ReadString(), BR.ReadInt32(), BR.ReadString(), BR.ReadString(), BR.ReadInt32());
                Person.Prosm();
            }
            BR.Close();
        }
        public static void prosmSov()
        {
            BinaryReader BR = new BinaryReader(File.Open("Authdata.txt", FileMode.OpenOrCreate));
            while (BR.PeekChar() != -1)
            {
                Person = new Auth_data(BR.ReadInt32(), BR.ReadString(), BR.ReadString(), BR.ReadInt32(), BR.ReadString(), BR.ReadString(), BR.ReadInt32());
                if(Person.age >= 18) Person.Prosm();
            }
            BR.Close();
        }
        public static void prosmFam(string Famuliya, string Name)
        {
            BinaryReader BR = new BinaryReader(File.Open("Authdata.txt", FileMode.OpenOrCreate));
            while (BR.PeekChar() != -1)
            {
                Person = new Auth_data(BR.ReadInt32(), BR.ReadString(), BR.ReadString(), BR.ReadInt32(), BR.ReadString(), BR.ReadString(), BR.ReadInt32());
                if ((Person.Name.ToLower() == Name.ToLower()) && (Person.Familiya.ToLower() == Famuliya.ToLower())) Person.Prosm();
            }
            BR.Close();
        }
    }
}
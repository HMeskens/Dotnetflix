using System;

namespace dotNETproject1
{
    class Program
    {
        private static void Main(string[] args)
        {
            OnlineStore onlineStore = new OnlineStore();
            onlineStore.MainMenu();
            Console.ReadLine();
        }
    }
}

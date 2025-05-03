using System;
using Pike.OneS.Install;
using System.EnterpriseServices.Internal;
using System.IO;

namespace Pike.OneS.ConsoleInstaller
{
    class Program
    {
        static readonly DbProviderInstallation DbInstallation = new DbProviderInstallation();
        static readonly WebServiceProviderInstallation WebServiceInstallation = new WebServiceProviderInstallation();

        static readonly string ProviderAssembly =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pike.OneS.dll");

        static readonly Publish Publish = new Publish();

        static void Main()
        {
            Console.WriteLine("Введите i для установки и u для удаления компонентов провайдера");
            var key = Console.ReadKey().KeyChar.ToString().ToLowerInvariant();

            if (key == "i") Install();
            if (key == "u") Uninstall();

            Console.WriteLine("Done!");
            Console.ReadKey();
        }

        public static void Install()
        {

            Console.WriteLine("Попытка установить провайдер в GAC...");
            try
            {
                
                Publish.GacInstall(ProviderAssembly);
                Console.WriteLine("Установка провайдера в GAC успешно завершена");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Невозможно установть проавйдер в GAC:");
                Console.WriteLine(exception.Message);
            }
            Console.WriteLine("=============================================");

            var machineConfig32 = new FileInfo(DataProviderInstallationBase.Machine32Config);
            var machineConfig64 = new FileInfo(DataProviderInstallationBase.Machine64Config);

            Console.WriteLine($"Ищем machineConfig32 = {machineConfig32.Exists} ({machineConfig32.FullName})");
            Console.WriteLine($"Ищем machineConfig64 = {machineConfig64.Exists} ({machineConfig64.FullName})");

            Console.WriteLine("Попытка регистрации провайдеров в machineConfig32...");
            try
            {
                DbInstallation.RegisterProvider(DataProviderInstallationBase.Machine32Config);
                WebServiceInstallation.RegisterProvider(DataProviderInstallationBase.Machine32Config);
                Console.WriteLine("Регистрация провайдеров в machineConfig32 успешно завершена");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Невозможно зарегистрировать провайдер в machineConfig32:");
                Console.WriteLine(exception.Message);
            }
            Console.WriteLine("=============================================");

            Console.WriteLine("Попытка регистрации провайдеров в machineConfig64...");
            try
            {
                DbInstallation.RegisterProvider(DataProviderInstallationBase.Machine64Config);
                WebServiceInstallation.RegisterProvider(DataProviderInstallationBase.Machine64Config);
                Console.WriteLine("Регистрация провайдеров в machineConfig64 успешно завершена");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Невозможно зарегистрировать провайдер в machineConfig64:");
                Console.WriteLine(exception.Message);
            }
            Console.WriteLine("=============================================");
        }

        public static void Uninstall()
        {
            Console.WriteLine("Попытка удалить провайдер из GAC...");
            try
            {
                Publish.GacRemove(ProviderAssembly);
                Console.WriteLine("Удаление провайдера из GAC успешно завершено");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Невозможно удалить проавйдер из GAC:");
                Console.WriteLine(exception.Message);
            }
            Console.WriteLine("=============================================");

            var machineConfig32 = new FileInfo(DataProviderInstallationBase.Machine32Config);
            var machineConfig64 = new FileInfo(DataProviderInstallationBase.Machine64Config);

            Console.WriteLine($"Ищем machineConfig32 = {machineConfig32.Exists} ({machineConfig32.FullName})");
            Console.WriteLine($"Ищем machineConfig64 = {machineConfig64.Exists} ({machineConfig64.FullName})");

            Console.WriteLine("Попытка удаления регистрации провайдеров в machineConfig32...");
            try
            {
                DbInstallation.UnregisterProvider(DataProviderInstallationBase.Machine32Config);
                WebServiceInstallation.UnregisterProvider(DataProviderInstallationBase.Machine32Config);
                Console.WriteLine("Удаление регистрации провайдеров в machineConfig32 успешно завершено");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Невозможно удалить регистрацию провайдеров в machineConfig32:");
                Console.WriteLine(exception.Message);
            }
            Console.WriteLine("=============================================");

            Console.WriteLine("Попытка удаления регистрации провайдеров в machineConfig64...");
            try
            {
                DbInstallation.UnregisterProvider(DataProviderInstallationBase.Machine64Config);
                WebServiceInstallation.UnregisterProvider(DataProviderInstallationBase.Machine64Config);
                Console.WriteLine("Удаление регистрации провайдеров в machineConfig64 успешно завершено");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Невозможно удалить регистрацию провайдеров в machineConfig64:");
                Console.WriteLine(exception.Message);
            }
            Console.WriteLine("=============================================");
        }
    }
}

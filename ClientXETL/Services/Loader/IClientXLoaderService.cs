using ClientXETL.Services.Storage;

namespace ClientXETL.Services.Loader
{
    public interface IClientXLoaderService
    {
        Task LoadAsync(CancellationToken cancellationToken);
    }

    public class DumpClientXAsTable(IPolicyStorage policyStorage, IRiskStorage riskStorage) : IClientXLoaderService
    {
        public Task LoadAsync(CancellationToken cancellationToken)
        {
            // Dump Policies to SQL script files as table-like structure into console
            Console.WriteLine("\nPolicies:\n");
            Console.WriteLine("ID".PadRight(10) + "PolicyName");
            Console.WriteLine(new string('-', 40)); // Separator line for clarity
            foreach (var policy in policyStorage.Policies)
            {
                Console.WriteLine($"{policy.ID.ToString().PadRight(10)}{policy.PolicyName}");
            }

            Console.WriteLine("\nRisks:\n");
            Console.WriteLine("ID".PadRight(10) +
                              "RiskName".PadRight(20) +
                              "Peril".PadRight(15) +
                              "PolicyID".PadRight(15) +
                              "Street".PadRight(20) +
                              "ClientId".PadRight(15) +
                              "Latitude".PadRight(15) +
                              "Longitude");
            Console.WriteLine(new string('-', 120)); // Separator line for clarity
            foreach (var risk in riskStorage.Risks)
            {
                Console.WriteLine($"{risk.ID.ToString().PadRight(10)}" +
                                  $"{risk.RiskName.PadRight(20)}" +
                                  $"{risk.Peril.ToString().PadRight(15)}" +
                                  $"{risk.PolicyID.ToString().PadRight(15)}" +
                                  $"{risk.Street.PadRight(20)}" +
                                  $"{risk.ClientID.ToString().PadRight(15)}" +
                                  $"{risk.Latitude.ToString().PadRight(15)}" +
                                  $"{risk.Longitude}");
            }
            Console.WriteLine();

            return Task.CompletedTask;
        }
    }

}

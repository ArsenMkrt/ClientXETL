using ClientXETL.Models;
using ClientXETL.Services.Storage;

namespace ClientXETL.Services.SearchIndexes;

public class PolicyIdSearchIndex(IPolicyStorage policyLoader)
    : SearchIndex<int, Policy>(policyLoader.Policies, p => p.ID)
{

}

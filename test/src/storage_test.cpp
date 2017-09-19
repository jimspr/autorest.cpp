#include "StorageManagementClient.h"
#include <autorest_oauth.h>
#include "client_info.h"

using namespace std;
using namespace awaituv;
using namespace awaitcurl;
using namespace autorest::json;
using namespace autorest::util;
using namespace autorest::oauth;

future_t<void> test1(curl_requester_t& requester)
{
  auto token_result = co_await get_azure_token_using_client_credentials(requester,
    directory_id,         // tenant (directory id)
    client_id,         // client-id TestApp1 application id
    client_secret, // client-secret TestApp1 key
    "https://management.azure.com/",                // resource
    {});

  token_result.print_response("get-token");
  if (!token_result.object.has_value())
    return;

  bearer_token &token = token_result.object.value();
  string &access_token = token.access_token.value();
  map<string, string> headers;
  headers["Authorization"] = string("Bearer ") + access_token;

  storage::StorageManagementClient client(requester, {});
  client.apiVersion = "2017-06-01";
  client.subscriptionId = subscription_id;

  auto result = co_await client.storageAccounts.List(headers);
  result.print_response("List");

  StorageAccountCheckNameAvailabilityParameters params{ "photos" };
  auto r2 = co_await client.storageAccounts.CheckNameAvailability(params, headers);
  r2.print_response("CheckNameAvailability");

  auto r6 = co_await client.storageAccounts.ListByResourceGroup(storage_resource_group, headers);
  r6.print_response("ListByResourceGroup");

  auto r5 = co_await client.storageAccounts.GetProperties(storage_resource_group, storage_account_name, headers);
  r5.print_response("GetProperties");

  auto r7 = co_await client.storageAccounts.ListKeys(storage_resource_group, storage_account_name, headers);
  r7.print_response("ListKeys");
  
  return;
}

void all_tests(curl_requester_t& requester, std::vector<string>& statuses)
{
  test1(requester);
}

int main(int argc, char* argv[])
{
  loop_t loop;
  curl_global_t global;
  curl_requester_t requester(loop);

  // process command line
  std::vector<string> statuses;
  for (int i = 1; i < argc; ++i)
  {
    if (strcmp(argv[i], "--verbose") == 0)
      requester.verbose = true;
    else
      statuses.push_back(argv[i]);
  }

  all_tests(requester, statuses);
  loop.run();

  return 0;
}

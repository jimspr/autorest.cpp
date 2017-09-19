#include "vaults.h"
#include "keyvaultmanagementclient.h"
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

  bearer_token& btoken = token_result.object.value();
  std::string& access_token = btoken.access_token.value();
  map<string, string> headers;
  headers["Authorization"] = string("Bearer ") + access_token;

  keyvault::KeyVaultManagementClient ps(requester, {});
  ps.apiVersion = "2016-10-01";
  ps.subscriptionId = subscription_id;

  auto result = co_await ps.vaults.List({}, headers);
  result.print_response("List");

  auto get = co_await ps.vaults.Get(vaults_resource_group, vault_name, headers);
  //auto& vault = get.object.value();
  get.print_response("Get");

  return;
}

void all_tests(curl_requester_t& requester)
{
  test1(requester);
}

int main(int argc, char* argv[])
{
  loop_t loop; // libuv event loop
  curl_global_t global;
  curl_requester_t requester(loop);

  for (int i = 1; i < argc; ++i)
  {
    if (strcmp(argv[i], "--verbose") == 0)
      requester.verbose = true;
  }

  all_tests(requester);
  loop.run();

  return 0;
}

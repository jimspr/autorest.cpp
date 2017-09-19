#include "documentdb.h"
#include <autorest_oauth.h>
#include "client_info.h"

using namespace std;
using namespace awaituv;
using namespace awaitcurl;
using namespace autorest::json;
using namespace autorest::util;
using namespace autorest::oauth;

future_t<void> test1(curl_requester_t &requester)
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
  documentdb::DocumentDB ps(requester, {});
  ps.apiVersion = "2015-04-08";
  ps.subscriptionId = subscription_id;

  auto result1 = ps.databaseAccounts.Get(resource_group, docs_account_name, headers);
  auto result2 = ps.databaseAccounts.List(headers);
  auto result3 = ps.databaseAccounts.ListByResourceGroup(resource_group, headers);
  auto result4 = ps.databaseAccounts.ListKeys(resource_group, docs_account_name, headers);
  auto result5 = ps.databaseAccounts.ListConnectionStrings(resource_group, docs_account_name, headers);
  auto result6 = ps.databaseAccounts.ListReadOnlyKeys(resource_group, docs_account_name, headers);
  
  vector<future_t<http_response_t>> v;
  v.push_back(ps.databaseAccounts.CheckNameExists(docs_account_name, headers));
  v.push_back(ps.databaseAccounts.CheckNameExists("foobar", headers));

  auto index = co_await future_of_any_range(v.begin(), v.end());
  auto result_vec = co_await future_of_all_range(v.begin(), v.end());

//  co_await future_of_any(result1, result2, result3, result4, result5, result6, result7, result8);

  co_await (result1 && result2);
  result1.get_value().print_response("Get");
  result2.get_value().print_response("List");

  co_await future_of_all(result1, result2, result3, result4, result5, result6);

  result3.get_value().print_response("ListByResourceGroup");
  result4.get_value().print_response("ListKeys");
  result5.get_value().print_response("ListConnectionStrings");
  result6.get_value().print_response("ListReadOnlyKeys");

  result_vec[0].print_response("CheckNameExists(files)");
  result_vec[1].print_response("CheckNameExists(foobar)");
  printf("index: %d\n", index - v.begin());

  return;
}

void all_tests(curl_requester_t &requester)
{
  test1(requester);
}

int main(int argc, char *argv[])
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

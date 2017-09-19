#include "AzureBlobStorage.h"
#include "Blobs.h"
#include <autorest_oauth.h>
#include <fcntl.h>
#include "client_info.h"

using namespace std;
using namespace awaituv;
using namespace awaitcurl;
using namespace autorest::json;
using namespace autorest::util;
using namespace autorest::oauth;

#if 0
std::string GetAuthenticationString(string& VERB, string& Content_Encoding, string& Content_Language, string& Content_Length, string& Content_MD5, string& Content_Type, string& Date, 
  string& If_Modified_Since, string& If_Match, string& If_None_Match, string& If_Unmodified_Since, string& Range, string& CanonicalizedHeaders, string& CanonicalizedResource)
{

  string storageAccountName = "x";
  string storageKey = "y";

  string StringToSign = VERB + "\n" +  
               Content_Encoding + "\n" +  
               Content_Language + "\n" +  
               Content_Length + "\n" +  
               Content_MD5 + "\n" +  
               Content_Type + "\n" +  
               Date + "\n" +  
               If_Modified_Since + "\n" +  
               If_Match + "\n" +  
               If_None_Match + "\n" +  
               If_Unmodified_Since + "\n" +  
               Range + "\n" +  
               CanonicalizedHeaders +   
               CanonicalizedResource;

}

std::string GetAuthenticationStringForTable(string& VERB, string& Content_Encoding, string& Content_Language, string& Content_Length, string& Content_MD5, string& Content_Type, string& Date,
  string& If_Modified_Since, string& If_Match, string& If_None_Match, string& If_Unmodified_Since, string& Range, string& CanonicalizedHeaders, string& CanonicalizedResource)
{
  std::string StringToSign = VERB + "\n" +
    Content_Encoding + "\n" +
    Content_Language + "\n" +
    Content_Length + "\n" +
    Content_MD5 + "\n" +
    Content_Type + "\n" +
    Date + "\n" +
    If_Modified_Since + "\n" +
    If_Match + "\n" +
    If_None_Match + "\n" +
    If_Unmodified_Since + "\n" +
    Range + "\n" +
    CanonicalizedHeaders +
    CanonicalizedResource;

}
#endif

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
  headers["Date"] = string("Fri, 14 Jul 2017 19:19:52 GMT");

  blob_storage::AzureBlobStorage service(requester, {});

  auto result = co_await service.blobs.Get(blob_account_name /*accountname*/, blob_container_name /*container*/, blob_name /*blob*/);
  result.print_response("Get");

  auto r2 = co_await service.blobs.GetProperties(blob_account_name, blob_container_name, blob_name);
  r2.print_response("GetProperties");

  auto r3 = co_await service.blobs.Put(blob_account_name, blob_container_name, "newblob", BlobType::blockBlob, { "foobar" });
  r3.print_response("Put");

#if 0
  auto r4 = co_await blobs.GetProperties(blob_account_name, blob_container_name, "newblob");
  r4.print_response("GetProperties");
#endif

  return;
}

future_t<void> test2(curl_requester_t& requester)
{
  blob_storage::AzureBlobStorage service(requester, {});

  auto result = co_await service.blobs.Get(blob_account_name /*accountname*/, blob_container_name /*container*/, blob_name /*blob*/);
  result.print_response("Get");

  auto r2 = co_await service.blobs.GetProperties(blob_account_name, blob_container_name, blob_name);
  r2.print_response("GetProperties");

  //uv_fs_t req;
  //auto file = co_await fs_open(&requester.loop, &req, "d:\\petstore.json", O_RDONLY, 0);

  //auto r3 = co_await blobs.Put(blob_account_name, blob_container_name, "newblob", BlobType::BlockBlob, &req);
  //r3.print_response("Put");

  auto r4 = co_await service.blobs.DeleteOperation(blob_account_name, blob_container_name, "newblob");
  r4.print_response("DeleteOperation");

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

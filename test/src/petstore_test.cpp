#include "swaggerpetstore.h"

using namespace std;
using namespace awaituv;
using namespace awaitcurl;
using namespace autorest::json;
using namespace autorest::util;

future_t<void> test_getall(curl_requester_t& requester)
{
  petstore::SwaggerPetstore ps(requester, {});
  auto result = co_await ps.GetInventory();
  result.print_response("GetInventory");

  return;
}

future_t<void> test_byid(curl_requester_t& requester, int64_t id)
{
  petstore::SwaggerPetstore ps(requester, {});

  auto result = co_await ps.GetPetById(id);
  result.print_response("GetPetById");

  return;
}

future_t<void> test_statuses(curl_requester_t& requester, std::vector<string>& statuses)
{
  petstore::SwaggerPetstore ps(requester, {});

  auto result = co_await ps.FindPetsByStatus(statuses);
  result.print_response("FindPetsByStatus");

  return;
}

future_t<void> test_tags(curl_requester_t& requester, std::vector<string>& tags)
{
  petstore::SwaggerPetstore ps(requester, {});

  auto result = co_await ps.FindPetsByTags(tags);
  result.print_response("FindPetsByTags");

  return;
}

future_t<void> test_create(curl_requester_t& requester, const std::string& name)
{
  petstore::SwaggerPetstore ps(requester, {});

  petstore::Models::Pet pet;
  pet.set_name(name.c_str());
  pet.set_status("msvc");
  auto result = co_await ps.AddPet(pet);
  result.print_response("AddPet");

  return;
}

future_t<void> test_delete(curl_requester_t& requester, uint64_t id)
{
  petstore::SwaggerPetstore ps(requester, {});

  auto result = co_await ps.DeletePet(id);
  result.print_response("DeletePet");

  return;
}

int main(int argc, char* argv[])
{
  loop_t loop;
  curl_global_t global;
  curl_requester_t requester(loop);

  // process command line
  std::vector<string> status;
  std::vector<string> tags;
  std::vector<string> ids;
  std::vector<string> create;
  std::vector<string> deletes;
  std::vector<string>* pvec = &status;

  for (int i = 1; i < argc; ++i)
  {
    if (strcmp(argv[i], "--verbose") == 0)
      requester.verbose = true;
    else if (strcmp(argv[i], "--status") == 0)
      pvec = &status;
    else if (strcmp(argv[i], "--tags") == 0)
      pvec = &tags;
    else if (strcmp(argv[i], "--ids") == 0)
      pvec = &ids;
    else if (strcmp(argv[i], "--create") == 0)
      pvec = &create;
    else if (strcmp(argv[i], "--delete") == 0)
      pvec = &deletes;
    else
      pvec->push_back(argv[i]);
  }

  test_getall(requester);
  if (!ids.empty())
  {
    for (auto& idstring : ids)
    {
      int64_t id = _atoi64(idstring.c_str());
      test_byid(requester, id);
    }
  }
  if (!status.empty())
    test_statuses(requester, status);
  if (!tags.empty())
    test_tags(requester, tags);
  for (auto& item : create)
    test_create(requester, item);
  for (auto& item : deletes)
    test_delete(requester, _atoi64(item.c_str()));

  loop.run();

  return 0;
}

cmake_minimum_required(VERSION 3.8)
set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /std:c++latest /await")

file(GLOB petstore_SRC generated/petstore/*.cpp generated/petstore/Models/*.cpp)
add_executable(petstore_test src/petstore_test.cpp ${petstore_SRC})
target_include_directories(petstore_test PUBLIC ./generated/petstore /autorest-clientruntime-for-cpp/inc /awaituv/awaituv/inc)

file(GLOB vaults_SRC generated/keyvault/*.cpp generated/keyvault/Models/*.cpp)
add_executable(keyvault_test src/keyvault_test.cpp ${vaults_SRC})
target_include_directories(keyvault_test PUBLIC ./generated/keyvault /autorest-clientruntime-for-cpp/inc /awaituv/awaituv/inc)

# documentdb not currently in Azure swagger repo
#file(GLOB documentdb_SRC generated/documentdb/*.cpp generated/documentdb/Models/*.cpp)
#add_executable(documentdb_test src/documentdb_test.cpp ${documentdb_SRC})
#target_include_directories(documentdb_test PUBLIC ./generated/documentdb /autorest-clientruntime-for-cpp/inc /awaituv/awaituv/inc)

file(GLOB iothub_SRC generated/iothub/*.cpp generated/iothub/Models/*.cpp)
add_executable(iothub_test src/iothub_test.cpp ${iothub_SRC})
target_include_directories(iothub_test PUBLIC ./generated/iothub /autorest-clientruntime-for-cpp/inc /awaituv/awaituv/inc)

file(GLOB logic_SRC generated/logic/*.cpp generated/logic/Models/*.cpp)
add_executable(logic_test src/logic_test.cpp ${logic_SRC})
target_include_directories(logic_test PUBLIC ./generated/logic /autorest-clientruntime-for-cpp/inc /awaituv/awaituv/inc)

file(GLOB machinelearning_SRC generated/machinelearning/*.cpp generated/machinelearning/Models/*.cpp)
add_executable(mach_test src/mach_test.cpp ${machinelearning_SRC})
target_include_directories(mach_test PUBLIC ./generated/machinelearning /autorest-clientruntime-for-cpp/inc /awaituv/awaituv/inc)

file(GLOB blob_SRC generated/blob_storage/*.cpp generated/blob_storage/Models/*.cpp)
add_executable(blob_test src/blob_test.cpp ${blob_SRC})
target_include_directories(blob_test PUBLIC ./generated/blob_storage /autorest-clientruntime-for-cpp/inc /awaituv/awaituv/inc)

file(GLOB advisor_SRC generated/advisor/*.cpp generated/advisor/Models/*.cpp)
add_executable(advisor_test src/advisor_test.cpp ${advisor_SRC})
target_include_directories(advisor_test PUBLIC ./generated/advisor /autorest-clientruntime-for-cpp/inc /awaituv/awaituv/inc)

file(GLOB analysisservices_SRC generated/analysisservices/*.cpp generated/analysisservices/Models/*.cpp)
add_executable(analysisservices_test src/analysisservices_test.cpp ${analysisservices_SRC})
target_include_directories(analysisservices_test PUBLIC ./generated/analysisservices /autorest-clientruntime-for-cpp/inc /awaituv/awaituv/inc)

file(GLOB authorization_SRC generated/authorization/*.cpp generated/authorization/Models/*.cpp)
add_executable(authorization_test src/authorization_test.cpp ${authorization_SRC})
target_include_directories(authorization_test PUBLIC ./generated/authorization /autorest-clientruntime-for-cpp/inc /awaituv/awaituv/inc)

file(GLOB automation_SRC generated/automation/*.cpp generated/automation/Models/*.cpp)
add_executable(automation_test src/automation_test.cpp ${automation_SRC})
target_include_directories(automation_test PUBLIC ./generated/automation /autorest-clientruntime-for-cpp/inc /awaituv/awaituv/inc)

#issue with some files not getting generated correctly
#file(GLOB batch_SRC generated/batch/*.cpp generated/batch/Models/*.cpp)
#add_executable(batch_test src/batch_test.cpp ${batch_SRC})
#target_include_directories(batch_test PUBLIC ./generated/batch /autorest-clientruntime-for-cpp/inc /awaituv/awaituv/inc)

file(GLOB cdn_SRC generated/cdn/*.cpp generated/cdn/Models/*.cpp)
add_executable(cdn_test src/cdn_test.cpp ${cdn_SRC})
target_include_directories(cdn_test PUBLIC ./generated/cdn /autorest-clientruntime-for-cpp/inc /awaituv/awaituv/inc)

file(GLOB compositeApiManagement_SRC generated/compositeApiManagement/*.cpp generated/compositeApiManagement/Models/*.cpp)
add_executable(compositeApiManagement_test src/compositeApiManagement_test.cpp ${compositeApiManagement_SRC})
target_include_directories(compositeApiManagement_test PUBLIC ./generated/compositeApiManagement /autorest-clientruntime-for-cpp/inc /awaituv/awaituv/inc)

file(GLOB compositeAppInsights_SRC generated/compositeAppInsights/*.cpp generated/compositeAppInsights/Models/*.cpp)
add_executable(appinsights_test src/appinsights_test.cpp ${compositeAppInsights_SRC})
target_include_directories(appinsights_test PUBLIC ./generated/compositeAppInsights /autorest-clientruntime-for-cpp/inc /awaituv/awaituv/inc)

file(GLOB storage_SRC generated/storage/*.cpp generated/storage/Models/*.cpp)
add_executable(storage_test src/storage_test.cpp ${storage_SRC})
target_include_directories(storage_test PUBLIC ./generated/storage /autorest-clientruntime-for-cpp/inc /awaituv/awaituv/inc)

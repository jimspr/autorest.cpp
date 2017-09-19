call autorest --use=/autorest.cpp --cpp --input-file=\other_swagger\petstore.json --output-folder=./generated/petstore/
call autorest --use=/autorest.cpp --cpp --input-file=\other_swagger\blob_storage.yaml --output-folder=./generated/blob_storage/
call autorest --use=/autorest.cpp --cpp --input-file=\azure-rest-api-specs\arm-keyvault\2016-10-01\swagger\keyvault.json --output-folder=./generated/keyvault/
rem call autorest --use=/autorest.cpp --cpp --input-file=\azure-rest-api-specs\arm-documentdb\2015-04-08\swagger\documentdb.json --output-folder=./generated/documentdb/
call autorest --use=/autorest.cpp --cpp --input-file=\azure-rest-api-specs\arm-iothub\2017-01-19\swagger\iothub.json --output-folder=./generated/iothub/
call autorest --use=/autorest.cpp --cpp --input-file=\azure-rest-api-specs\arm-advisor\2017-04-19\swagger\advisor.json --output-folder=./generated/advisor/
call autorest --use=/autorest.cpp --cpp --input-file=\azure-rest-api-specs\arm-analysisservices\2016-05-16\swagger\analysisservices.json --output-folder=./generated/analysisservices/
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeApiManagement --input-file=\azure-rest-api-specs\arm-apimanagement\2016-10-10\swagger\apimanagement.json --namespace=apim
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeApiManagement --input-file=\azure-rest-api-specs\arm-apimanagement\2016-10-10\swagger\apimapis.json --namespace=apim
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeApiManagement --input-file=\azure-rest-api-specs\arm-apimanagement\2016-10-10\swagger\apimauthorizationservers.json --namespace=apim
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeApiManagement --input-file=\azure-rest-api-specs\arm-apimanagement\2016-10-10\swagger\apimbackends.json --namespace=apim
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeApiManagement --input-file=\azure-rest-api-specs\arm-apimanagement\2016-10-10\swagger\apimcertificates.json --namespace=apim
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeApiManagement --input-file=\azure-rest-api-specs\arm-apimanagement\2016-10-10\swagger\apimdeployment.json --namespace=apim
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeApiManagement --input-file=\azure-rest-api-specs\arm-apimanagement\2016-10-10\swagger\apimgroups.json --namespace=apim
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeApiManagement --input-file=\azure-rest-api-specs\arm-apimanagement\2016-10-10\swagger\apimidentityprovider.json --namespace=apim
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeApiManagement --input-file=\azure-rest-api-specs\arm-apimanagement\2016-10-10\swagger\apimloggers.json --namespace=apim
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeApiManagement --input-file=\azure-rest-api-specs\arm-apimanagement\2016-10-10\swagger\apimnetworkstatus.json --namespace=apim
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeApiManagement --input-file=\azure-rest-api-specs\arm-apimanagement\2016-10-10\swagger\apimopenidconnectproviders.json --namespace=apim
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeApiManagement --input-file=\azure-rest-api-specs\arm-apimanagement\2016-10-10\swagger\apimproducts.json --namespace=apim
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeApiManagement --input-file=\azure-rest-api-specs\arm-apimanagement\2016-10-10\swagger\apimproperties.json --namespace=apim
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeApiManagement --input-file=\azure-rest-api-specs\arm-apimanagement\2016-10-10\swagger\apimquotas.json --namespace=apim
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeApiManagement --input-file=\azure-rest-api-specs\arm-apimanagement\2016-10-10\swagger\apimreports.json --namespace=apim
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeApiManagement --input-file=\azure-rest-api-specs\arm-apimanagement\2016-10-10\swagger\apimsubscriptions.json --namespace=apim
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeApiManagement --input-file=\azure-rest-api-specs\arm-apimanagement\2016-10-10\swagger\apimtenant.json --namespace=apim
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeApiManagement --input-file=\azure-rest-api-specs\arm-apimanagement\2016-10-10\swagger\apimusers.json --namespace=apim

call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeAppInsights --input-file=\azure-rest-api-specs\arm-appinsights\2015-05-01\swagger\aiOperations_API.json --namespace=appin
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeAppInsights --input-file=\azure-rest-api-specs\arm-appinsights\2015-05-01\swagger\components_API.json --namespace=appin
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/compositeAppInsights --input-file=\azure-rest-api-specs\arm-appinsights\2015-05-01\swagger\webTests_API.json --namespace=appin

call autorest --use=/autorest.cpp --cpp --input-file=\azure-rest-api-specs\arm-authorization\2015-07-01\swagger\authorization.json --output-folder=./generated/authorization

call autorest --use=/autorest.cpp --cpp --output-folder=./generated/automation --input-file=\azure-rest-api-specs\arm-automation\2015-10-31\swagger\account.json --namespace=automation
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/automation --input-file=\azure-rest-api-specs\arm-automation\2015-10-31\swagger\certificate.json --namespace=automation
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/automation --input-file=\azure-rest-api-specs\arm-automation\2015-10-31\swagger\connection.json --namespace=automation
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/automation --input-file=\azure-rest-api-specs\arm-automation\2015-10-31\swagger\connectionType.json --namespace=automation
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/automation --input-file=\azure-rest-api-specs\arm-automation\2015-10-31\swagger\credential.json --namespace=automation
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/automation --input-file=\azure-rest-api-specs\arm-automation\2015-10-31\swagger\definitions.json --namespace=automation
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/automation --input-file=\azure-rest-api-specs\arm-automation\2015-10-31\swagger\dscCompilationJob.json --namespace=automation
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/automation --input-file=\azure-rest-api-specs\arm-automation\2015-10-31\swagger\dscConfiguration.json --namespace=automation
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/automation --input-file=\azure-rest-api-specs\arm-automation\2015-10-31\swagger\dscNode.json --namespace=automation
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/automation --input-file=\azure-rest-api-specs\arm-automation\2015-10-31\swagger\dscNodeConfiguration.json --namespace=automation
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/automation --input-file=\azure-rest-api-specs\arm-automation\2015-10-31\swagger\hybridRunbookWorkerGroup.json --namespace=automation
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/automation --input-file=\azure-rest-api-specs\arm-automation\2015-10-31\swagger\job.json --namespace=automation
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/automation --input-file=\azure-rest-api-specs\arm-automation\2015-10-31\swagger\jobSchedule.json --namespace=automation
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/automation --input-file=\azure-rest-api-specs\arm-automation\2015-10-31\swagger\module.json --namespace=automation
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/automation --input-file=\azure-rest-api-specs\arm-automation\2015-10-31\swagger\runbook.json --namespace=automation
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/automation --input-file=\azure-rest-api-specs\arm-automation\2015-10-31\swagger\schedule.json --namespace=automation
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/automation --input-file=\azure-rest-api-specs\arm-automation\2015-10-31\swagger\variable.json --namespace=automation
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/automation --input-file=\azure-rest-api-specs\arm-automation\2015-10-31\swagger\webhook.json --namespace=automation

rem call autorest --use=/autorest.cpp --cpp --output-folder=./generated/batch --input-file=\azure-rest-api-specs\arm-batch\2017-05-01\swagger\BatchManagement.json

call autorest --use=/autorest.cpp --cpp --output-folder=./generated/cdn --input-file=\azure-rest-api-specs\arm-cdn\2016-10-02\swagger\cdn.json

call autorest --use=/autorest.cpp --cpp --output-folder=./generated/logic --input-file=\azure-rest-api-specs\arm-logic\2016-06-01\swagger\logic.json
call autorest --use=/autorest.cpp --cpp --output-folder=./generated/machinelearning --input-file=\azure-rest-api-specs\arm-machinelearning\2017-01-01\swagger\webservices.json

call autorest --use=/autorest.cpp --cpp --output-folder=./generated/storage --input-file=\azure-rest-api-specs\arm-storage\2016-12-01\swagger\storage.json

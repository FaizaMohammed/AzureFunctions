
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using Newtonsoft.Json;
using StorageAccessFunction.AzureFunctions;
using StorageAccessFunction.Repositories.Interfaces;
using StorageAccessFunction.Services;
using StorageAccessFunction.Services.Interfaces;
using StorageAccessFunction.TableRepo;
using Microsoft.Extensions.Logging;

namespace TestStorageAccessFunction
{
    public class Tests
	{
		private PushToRepo _pushToRepo;
		private Mock<IConfiguration> _configurationMock;
		//private Mock<IKeyVaultRepository> _keyVaultRepositoryMock;
		//private Mock<IPmsRepository> _pmsRepositoryMock;
		private Mock<HttpRequest> _httpRequestMock;
		private Mock<Microsoft.Extensions.Logging.ILogger> _loggerMock;
		private Mock<ICloudStorageAccountProvider> _cloudStorageAccountProviderMock;
		private Mock<IStorageCredentialsProvider> _storageCredentialsProviderMock;
		private Mock<ICloudTableClientProvider> _cloudTableClientProviderMock;
		private Mock<IPmsRepository> _tableActionMock;
		private Mock<IEnvtTableNameProvider> _envtTableNameProviderMock;
		private Mock<ITableProvider> _tableProviderMock;
		private Mock<ITableReferenceProvider> _tableReferenceProviderMock;



		public Pmsrepository ta;
		[SetUp]
		public void Setup()
		{
			var mockUri = new Uri("https://example.com");
			_configurationMock = new Mock<IConfiguration>();
			
			
			_cloudStorageAccountProviderMock = new Mock<ICloudStorageAccountProvider>();
			_envtTableNameProviderMock = new Mock<IEnvtTableNameProvider>();
			_tableActionMock = new Mock<IPmsRepository>();
			_cloudTableClientProviderMock = new Mock<ICloudTableClientProvider>();
			_tableProviderMock = new Mock<ITableProvider>();
			_tableReferenceProviderMock = new Mock<ITableReferenceProvider>();
			var cloudTableMock = new Mock<CloudTable>(new Uri("http://unittests.localhost.com/FakeTable"));
			
			// Set up the necessary configuration values
			_configurationMock.Setup(c => c["AzureStorageConnectionString"]).Returns("DefaultEndpointsProtocol=https;AccountName=storageaccountfaiz;AccountKey=SBeBHvkuojVtWpYN+mi3BS60kCiiwaJsd/NjftgkTvJ8Q7koiylpoO85EOpCiuH5/AaAPJQFELVx+ASt/CXSUg==;EndpointSuffix=core.windows.net");
			_configurationMock.Setup(c => c["KEY_VAULT_NAME"]).Returns("testkyvault-001");
			 var mockCredentials = new StorageCredentials("test", "test");
			var mockCloudTableClient = new Mock<CloudTableClient>(new Uri("http://unittests.localhost.com/FakeTable"), mockCredentials);
			_storageCredentialsProviderMock = new Mock<IStorageCredentialsProvider>();
			_storageCredentialsProviderMock.Setup(p => p.GetStorageCredentials()).Returns(mockCredentials);
			_tableProviderMock.Setup(tp => tp.getTable()).Returns(mockCloudTableClient.Object);
			_envtTableNameProviderMock.Setup(etnp => etnp.getTableName()).Returns("mockTableName");
			mockCloudTableClient.Setup(ctc => ctc.GetTableReference("mockTableName")).Returns(cloudTableMock.Object);
			
			_httpRequestMock = new Mock<HttpRequest>();
			_loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger>();
			_pushToRepo = new PushToRepo(_tableActionMock.Object);



		}

		[Test]
		public async Task Test_case_for_successful_retrieval()
		{
			ta = new Pmsrepository(_tableProviderMock.Object, _envtTableNameProviderMock.Object);
			var ret=await ta.GetDataAsync();
			Assert.IsInstanceOf<List<TableData>>(ret);
		}
		[Test]
		public Task  Test_case_for_successful_Insertion()
		{
			var tableData = new TableData();
			{
				tableData.client_id = "4";
				tableData.scope = "TestDatat2";
				tableData.PartitionKey = "llry";
				tableData.RowKey = "TestRo4wKhey123";
			};
			ta = new Pmsrepository(_tableProviderMock.Object, _envtTableNameProviderMock.Object);
			var data =  ta.InsertEntity(tableData);
			Assert.That(data, Is.Not.Null);
			return Task.CompletedTask;
		}

		[Test]
		public async Task Test_case_for_nonsuccessful_Insertion()
		//InsertEntity<T>(T entity, bool forInsert = true)
		{
			var tableData = new TableData();
			tableData = null;
			ta = new Pmsrepository(_tableProviderMock.Object, _envtTableNameProviderMock.Object);
		
			Assert.ThrowsAsync<ArgumentNullException>(async () => await ta.InsertEntity<TableData>(tableData));
		}

		[Test]
		public async Task Run_ValidRequestBody_ReturnsOkObjectResult()
		{
			// Arrange
			string clientId = "testClientId";
			string clientSecret = "testClientSecret";
			string scope = "testScope";

			dynamic requestBody = new
			{
				client_id = clientId,
				//client_secret = clientSecret,
				scope = scope
			};
			  

			string serializedRequestBody = JsonConvert.SerializeObject(requestBody);
			MemoryStream requestBodyStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(serializedRequestBody));

			_httpRequestMock.Setup(r => r.Body).Returns(requestBodyStream);
			
			
			var tableData = new TableData();
			{
				tableData.client_id = "4";
				tableData.scope = "TestDatat2";
				tableData.PartitionKey = "llry";
				tableData.RowKey = "TestRo4wKhey123";
			};
			var list= new List<TableData>();
			list.Add(tableData);
			_tableActionMock.Setup(ta => ta.InsertEntity(It.IsAny<TableData>(),true)).Returns(Task.CompletedTask);
			_tableActionMock.Setup(ta => ta.GetDataAsync()).ReturnsAsync(list);

			// Act
			IActionResult result = await _pushToRepo.Run(_httpRequestMock.Object, _loggerMock.Object) as OkObjectResult;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result, Is.InstanceOf<OkObjectResult>());
			// Perform additional assertions on the OkObjectResult if needed
			// Act
		//	IActionResult res = await _pushToRepo.Run(_httpRequestMock.Object, (Microsoft.Extensions.Logging.ILogger)_loggerMock);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			OkObjectResult okResult = (OkObjectResult)result;
			var responseMessage = (List<TableData>)okResult.Value;

			OkObjectResult oksult = (OkObjectResult)result;
			Assert.That(responseMessage, Is.EqualTo(list));

		}
		[Test]
		public async Task Run_InvalidJsonPayload_ReturnsBadRequestObjectResult()
		{
			// Arrange
			string requestBody =  "invalidJson" ;
			MemoryStream requestBodyStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(requestBody));

			_httpRequestMock.Setup(r => r.Body).Returns(requestBodyStream);

			// Act
			IActionResult result = await _pushToRepo.Run(_httpRequestMock.Object, _loggerMock.Object) as BadRequestObjectResult;

			// Assert
			Assert.IsInstanceOf<BadRequestObjectResult>(result);
			BadRequestObjectResult badRequestResult = (BadRequestObjectResult)result;
			string responseMessage = (string)badRequestResult.Value;
			Assert.That(responseMessage, Is.EqualTo("Invalid JSON payload."));
			//_keyVaultRepositoryMock.Verify(k => k.SetSecret(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
			//_pmsRepositoryMock.Verify(p => p.InsertPmsData(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
		}

	}
}
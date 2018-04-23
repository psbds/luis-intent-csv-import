using Psbds.LUIS.IntentCSVImport.Core.Helpers;
using Psbds.LUIS.IntentCSVImport.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psbds.LUIS.IntentCSVImport.Core.Helpers;

namespace Psbds.LUIS.IntentCSVImport.Core
{
    public class ImportService
    {
        private readonly string _applicationId;
        private readonly string _applicationVersion;
        private readonly LuisClient _luisClient;

        public ImportService(string applicationId, string applicationKey, string applicationVersion)
        {
            _applicationId = applicationId;
            _applicationVersion = applicationVersion;
            _luisClient = new LuisClient(applicationKey);
        }

        public async Task<bool> ImportExamples(List<Tuple<string, string>> utterances)
        {

            var applicationVersionModel = (await this._luisClient.ExportVersion(_applicationId, _applicationVersion)).DeserializeObject<ApplicationVersionModel>();

            var fileIntents = utterances.Select(x => x.Item1).Distinct();

            var intentsToCreate = from a in fileIntents
                                  where !applicationVersionModel.Intents.Select(b => b.Name).Distinct().Contains(a)
                                  select a;

            Console.WriteLine("#### Creating New Intents");

            var intentChunks = intentsToCreate.Distinct().ToList().SplitList(5);

            foreach (var intentChunk in intentChunks)
            {
                var taskCreateNewIntents = intentChunk.Select(intent => Task.Run(() =>
                {
                    Console.WriteLine($"Creating Intent {intent}");
                    return _luisClient.CreateIntent(_applicationId, _applicationVersion, new ApplicationVersionIntentModel() { Name = intent });
                })).ToArray();

                Task.WaitAll(taskCreateNewIntents);
            }


            Console.WriteLine("#### Importing Examples");
            var batchExamples = utterances.Select(x => new UtteranceModel()
            {
                Text = x.Item2,
                IntentName = x.Item1
            }).ToList();

            var chunks = batchExamples.SplitList(99);

            foreach (var chunk in chunks)
            {
                var response = await _luisClient.BatchAddUtterance(_applicationId, _applicationVersion, chunk.ToArray());
            }

            return true;
        }

    }
}

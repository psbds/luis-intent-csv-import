using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psbds.LUIS.IntentCSVImport.Core.Models
{
    [Serializable]
    public class UtteranceEntityLabelModel
    {

        [JsonProperty("entityName")]
        public string EntityName { get; set; }

        [JsonProperty("startCharIndex")]
        public int StartCharIndex { get; set; }

        [JsonProperty("endCharIndex")]
        public int EndCharIndex { get; set; }
    }

    [Serializable]
    public class UtteranceModel
    {

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("intentName")]
        public string IntentName { get; set; }

        [JsonProperty("entityLabels")]
        public UtteranceEntityLabelModel[] EntityLabels { get; set; } = new UtteranceEntityLabelModel[] { };
    }
}

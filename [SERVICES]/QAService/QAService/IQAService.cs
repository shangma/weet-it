﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using VDS.RDF;
using VDS.RDF.Query;

namespace QAService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IQAService
    {
        [OperationContract]
        List<questionAnswer> GetAnswerWithQuestionStructure(string question);

        [OperationContract]
        List<PartialAnswer> GetPartialAnswer(string question);
    }
}

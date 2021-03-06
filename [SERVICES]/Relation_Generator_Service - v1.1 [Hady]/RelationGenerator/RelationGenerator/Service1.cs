﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RelationGenerator
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class RelationGeneratorService : IRelationGeneratorService
    {
        public List<List<string>> simpleGetRelations(List<string> uri, int Distance, int Limit = 50)
        {
            return RelFinder.getRelations(uri, Distance, Limit);
        }

        public List<List<KeyValuePair<string, string>>> simpleGetRelationWithLabels(List<string> uri, int Distance, int Limit = 50)
        {
            return RelFinder.getRelationWithLabels(uri, Distance, Limit);
        }

        public List<relation> getRelations(List<string> uri, int Distance, int Limit = 50)
        {
            List<List<string>> results = RelFinder.getRelations(uri, Distance, Limit);
            List<relation> relations = new List<relation>();
            foreach (List<string> s in  results)
            {
                relations.Add(new relation(s));
            }
            return relations;
        }

        public List<relation> getRelationWithLabels(List<string> uri, int Distance, int Limit = 50)
        {
            List<List<KeyValuePair<string,string>>> results =  RelFinder.getRelationWithLabels(uri, Distance, Limit);
            List<relation> relations = new List<relation>();
            foreach (List<KeyValuePair<string,string>> s in results)
            {
                relations.Add(new relation(s));
            }

            return relations;
        }


    }
}

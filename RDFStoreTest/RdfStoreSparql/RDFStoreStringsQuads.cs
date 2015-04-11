using System.Collections.Generic;
using System.IO;
using System.Linq;
using PolarDB;
using RdfStoreSparqlNamespace;
using SparqlParseRun.RdfCommon;    
    public class RDFStoreStringsQuads  :QuadGraph, IStore  , IStoreNamedGraphs  
    {
        public PaCell spogdTable;

        public StringNodeGenerator nodeGenerator;
      
        #region indexes

        private IndexByOrderedSet<string, string, ComparableObject, string> spog;
        private IndexByOrderedSet<string, ComparableObject, string> gop;
        private IndexByOrderedSet<ComparableObject, string, string> osg;
        private IndexByOrderedSet<string, string, string> pgs;
        private IndexByOrderedSet<string, ComparableObject> po;
        private IndexByOrderedSet<string, string> sg;

        private static readonly PTypeSequence spogdType = new PTypeSequence(new PTypeRecord(new NamedType("s", new PType(PTypeEnumeration.sstring)),
            new NamedType("p", new PType(PTypeEnumeration.sstring)),
            new NamedType("o", new PTypeUnion(
                new NamedType("uri", new PType(PTypeEnumeration.sstring)),
                new NamedType("bool", new PType(PTypeEnumeration.boolean)),
                new NamedType("str", new PType(PTypeEnumeration.sstring)),
                new NamedType("str", new PType(PTypeEnumeration.sstring)),
                new NamedType("lang str", new PTypeRecord(new NamedType("str", new PType(PTypeEnumeration.sstring)), new NamedType("lang", new PType(PTypeEnumeration.sstring)))),
                new NamedType("double", new PType(PTypeEnumeration.real)),
                new NamedType("decimal", new PType(PTypeEnumeration.real)),
                new NamedType("float", new PType(PTypeEnumeration.real)),
                new NamedType("int", new PType(PTypeEnumeration.integer)),
                new NamedType("date time offset", new PType(PTypeEnumeration.longinteger)),
                new NamedType("date time", new PType(PTypeEnumeration.longinteger)),
                new NamedType("time", new PType(PTypeEnumeration.longinteger)),
                new NamedType("typed", new PTypeRecord(new NamedType("str", new PType(PTypeEnumeration.sstring)), new NamedType("type", new PType(PTypeEnumeration.sstring)))))),
            new NamedType("g", new PType(PTypeEnumeration.sstring)),
            new NamedType("d", new PType(PTypeEnumeration.boolean))));

        private readonly string spogTableFilePath;
        private long deletes;
        
        #endregion

        public RDFStoreStringsQuads()
            : base()
        {
            spogTableFilePath = storeSettings.Default.root_dir_path + storeSettings.Default.spog_table_file_name;
            Open();
        }

        private void Open()
        {
            spogdTable = new PaCell(spogdType, spogTableFilePath, false);
            NodeGenerator = StringNodeGenerator.Create();
            base.graphUri = NodeGenerator.CreateUriNode(new UriPrefixed("dg:", "", storeSettings.Default.DefualtGraph));
            base.store = this;
            base.thisDataSet = new DataSet() { graphUri };
            if (spogdTable.IsEmpty) spogdTable.Fill(new object[0]);
            spog =
                new IndexByOrderedSet<string, string, ComparableObject,string>(
                    new DirectoryInfo(storeSettings.Default.root_dir_path + "spog"), spogdTable.Root,
                    storeSettings.Default.Indexes_max_Changes_For_Rebuild,
                    e => (string)e.Field(0).Get(), e => (string)e.Field(1).Get(), e => nodeGenerator.Writable2ComparableObject(e.Field(2).Get()),
                    e => (string)e.Field(3).Get());

            gop =
             new IndexByOrderedSet<string, ComparableObject, string>(
                 new DirectoryInfo(storeSettings.Default.root_dir_path + "gop"), spogdTable.Root,
                 storeSettings.Default.Indexes_max_Changes_For_Rebuild,
                 e => (string)e.Field(3).Get(), e => nodeGenerator.Writable2ComparableObject(e.Field(2).Get()), e => (string)e.Field(1).Get());

            osg =
             new IndexByOrderedSet<ComparableObject, string, string>(
                 new DirectoryInfo(storeSettings.Default.root_dir_path + "osg"), spogdTable.Root,
                 storeSettings.Default.Indexes_max_Changes_For_Rebuild,
                 e => nodeGenerator.Writable2ComparableObject(e.Field(2).Get()), e => (string)e.Field(0).Get(), e => (string)e.Field(3).Get());

            pgs =
             new IndexByOrderedSet<string, string, string>(
                 new DirectoryInfo(storeSettings.Default.root_dir_path + "pgs"), spogdTable.Root,
                 storeSettings.Default.Indexes_max_Changes_For_Rebuild,
                 e => (string)e.Field(1).Get(), e => (string)e.Field(3).Get(), e => (string)e.Field(0).Get());

            po =
             new IndexByOrderedSet<string, ComparableObject>(
                 new DirectoryInfo(storeSettings.Default.root_dir_path + "po"), spogdTable.Root,
                 storeSettings.Default.Indexes_max_Changes_For_Rebuild,
                 e => (string)e.Field(1).Get(), e => nodeGenerator.Writable2ComparableObject(e.Field(2).Get()));

            sg =
             new IndexByOrderedSet<string, string>(
                 new DirectoryInfo(storeSettings.Default.root_dir_path + "sg"), spogdTable.Root,
                 storeSettings.Default.Indexes_max_Changes_For_Rebuild,
                 e => (string)e.Field(0).Get(), e => (string)e.Field(3).Get());

            
            
            //{
            //    var row = (object[]) o;
            //    return row[0], row[1],
            //        nodeGenerator.Writable2ComparableObject(row[2])));
            //}, storeSettings.Default.Indexes_max_Changes_For_Rebuild);
            //gop = new IterationIndexMultiplyValues(new DirectoryInfo(storeSettings.Default.root_dir_path + "gop"), spogdTable.Root, o =>
            //{
            //    var row = (object[]) o;
            //    return row[3], nodeGenerator.Writable2ComparableObject(row[2])), row[1]);
            //}, 3,storeSettings.Default.Indexes_max_Changes_For_Rebuild);
            //osg = new IterationIndexMultiplyValues(new DirectoryInfo(storeSettings.Default.root_dir_path + "osg"), spogdTable.Root, o =>
            //{
            //    var row = (object[]) o;
            //    return nodeGenerator.Writable2ComparableObject(row[2])), row[0], row[3]);
            //}, 3, storeSettings.Default.Indexes_max_Changes_For_Rebuild);
            //pgs = new IterationIndexMultiplyValues(new DirectoryInfo(storeSettings.Default.root_dir_path + "pgs"), spogdTable.Root, o =>
            //{
            //    var row = (object[]) o;
            //    return row[1], row[3], row[0]);
            //}, 3, storeSettings.Default.Indexes_max_Changes_For_Rebuild);
            //po = new IterationIndexMultiplyValues(new DirectoryInfo(storeSettings.Default.root_dir_path + "po"), spogdTable.Root, o =>
            //{
            //    var row = (object[]) o;
            //    return row[1], nodeGenerator.Writable2ComparableObject(row[2])));
            //}, 2, storeSettings.Default.Indexes_max_Changes_For_Rebuild);
            //sg = new IterationIndexMultiplyValues(new DirectoryInfo(storeSettings.Default.root_dir_path + "sg"), spogdTable.Root, o =>
            //{
            //    var row = (object[]) o;
            //    return row[0], row[3]);
            //}, 2, storeSettings.Default.Indexes_max_Changes_For_Rebuild);
           
        }

        public new virtual INodeGenerator NodeGenerator
        {
            get { return nodeGenerator; }
            set { nodeGenerator = (StringNodeGenerator) value; }
        }

        public void RemoveDeletedFromTable()
        {
            if (deletes*100/spogdTable.Root.Count() < storeSettings.Default.max_deletes_for_recreate_table_persent)
                return;
            if (spogdTable.IsEmpty) return;

            string tempFilePath = storeSettings.Default.root_dir_path + "temp.pac";
            if (File.Exists(tempFilePath)) File.Delete(tempFilePath);
            var tempCell = new PaCell(spogdType, tempFilePath, false);
            tempCell.Fill(new object[0]);
            spogdTable.Root.Scan(o =>
            {
                if (!(bool) ((object[]) o)[4])
                    tempCell.Root.AppendElement(o);
                return true;
            });
            tempCell.Close();
            spogdTable.Close();
            File.Delete(spogTableFilePath);
            File.Move(tempFilePath, spogTableFilePath);
            nodeGenerator.Close();
            //  nodeGenerator = new CodedNodeGenerator(path);
            spog.Close();
            gop.Close();
            osg.Close();
            pgs.Close();
            po.Close();
            sg.Close();
            Open();
            deletes = 0;
        }

        #region Interfaces    

        public IStoreNamedGraphs NamedGraphs
        {
            get { return this; }
        }


        #region Get from named graphs

        public IEnumerable<IGrouping<IUriNode, INode>> GetTriplesWithSubjectPredicateFromGraphs(ISubjectNode subjectNode, IUriNode predicateNode, DataSet graphs)
        {
            if (!graphs.Any())
                return spog.Search(subjectNode.ToString(), predicateNode.ToString())
                    .Select(o => ((object[]) o))
                    .Where(row => !(bool)row[4])
                    .GroupBy(row => (string)row[3])
                    .Select(group =>
                        new Grouping<IUriNode, INode>(nodeGenerator.GetUri(group.Key), 
                                group.Select(o => o[2])
                                      .Cast<string>()
                                      .Select(nodeGenerator.GetUri)));
            else
                return graphs.Select(g => new Grouping<IUriNode, INode>(g,
                    pgs.Search(predicateNode.ToString(), g.ToString(), subjectNode.ToString())
                        .Where(o => !(bool) ((object[])o)[4])
                        .Select(o => ((object[])o)[2])
                        .Select(nodeGenerator.WritableObject2Node)));
        }

        public IEnumerable<IGrouping<IUriNode, IUriNode>> GetTriplesWithSubjectObjectFromGraphs(ISubjectNode subjectNode, INode objectNode, DataSet graphs)
        {
            if (!graphs.Any())
                return osg.Search(nodeGenerator.Node2ComparableObject(objectNode), subjectNode.ToString())
                    .Select(o => ((object[])o))
                    .Where(row => !(bool)row[4])
                    .GroupBy(row => (string)row[3])
                    .Select(group => new Grouping<IUriNode, IUriNode>(nodeGenerator.GetUri(group.Key),
                                group.Select(o => o[1])
                                      .Cast<string>()
                                      .Select(nodeGenerator.GetUri)));
            return graphs.Select(g => new Grouping<IUriNode, IUriNode>(g,
                           osg.Search(nodeGenerator.Node2ComparableObject(objectNode), subjectNode.ToString(), g.ToString())
                                   .Where(o => !(bool)((object[])o)[4])
                                   .Select(o => ((object[])o)[1])
                                   .Cast<string>()
                                   .Select(nodeGenerator.GetUri)));
        }
           public IEnumerable<IGrouping<IUriNode, ISubjectNode>> GetTriplesWithPredicateObjectFromGraphs(IUriNode predicateNode, INode objectNode, DataSet graphs)
        {
            if (!graphs.Any())
                return po.Search(predicateNode.ToString(), nodeGenerator.Node2ComparableObject(objectNode))
                    .Select(o => ((object[])o))
                    .Where(row => !(bool)row[4])
                    .GroupBy(row => (string)row[3])
                    .Select(group =>
                        new Grouping<IUriNode, ISubjectNode>(nodeGenerator.GetUri(group.Key),
                                group.Select(o => o[0])
                                      .Cast<string>()
                                      .Select(nodeGenerator.GetUri)));
            return graphs.Select(g => new Grouping<IUriNode, IUriNode>(g,
                              gop.Search(g.ToString(), nodeGenerator.Node2ComparableObject(objectNode), predicateNode.ToString())
                                        .Where(o => !(bool)((object[])o)[4])
                                      .Select(o => ((object[])o)[0])
                                      .Cast<string>()
                                      .Select(nodeGenerator.GetUri)));
        }
        public IEnumerable<IGrouping<IUriNode, Triple>> GetTriplesWithSubjectFromGraphs(ISubjectNode subjectNode, DataSet graphs)
        {
            if (!graphs.Any())
                return sg.Search(subjectNode.ToString())
                    .Select(o => ((object[])o))
                    .Where(row => !(bool)row[4])
                    .GroupBy(row => (string)row[3])
                    .Select(group =>
                        new Grouping<IUriNode, Triple>(nodeGenerator.GetUri(group.Key),
                                group.Select(nodeGenerator.ConvertToTriple)));
            return graphs.Select(g => new Grouping<IUriNode, Triple>(g,
                sg.Search(subjectNode.ToString(), g.ToString())
                    .Where(o => !(bool)((object[])o)[4])
                    .Select(o => ((object[])o))
                    .Select(nodeGenerator.ConvertToTriple)));
        }

      

        public IEnumerable<IGrouping<IUriNode, Triple>> GetTriplesWithPredicateFromGraphs(IUriNode predicateNode, DataSet graphs)
        {
            if (!graphs.Any())
                return po.Search(predicateNode.ToString())
                    .Select(o => ((object[])o))
                    .Where(row => !(bool)row[4])
                    .GroupBy(row => (string)row[3])
                    .Select(group =>
                        new Grouping<IUriNode, Triple>(nodeGenerator.GetUri(group.Key),
                                group.Select(nodeGenerator.ConvertToTriple)));
            return graphs.Select(g => new Grouping<IUriNode, Triple>(g,
                            pgs.Search(predicateNode.ToString(), g.ToString())
                                    .Where(o => !(bool)((object[])o)[4])
                                    .Select(o => ((object[])o))
                                    .Select(nodeGenerator.ConvertToTriple)));
        }

        public IEnumerable<IGrouping<IUriNode, Triple>> GetTriplesWithObjectFromGraphs(INode objectNode, DataSet graphs)
        {
            var node2ComparableObject = nodeGenerator.Node2ComparableObject(objectNode);
            if (!graphs.Any())
                return osg.Search(node2ComparableObject)
                    .Select(o => ((object[])o))
                    .Where(row => !(bool)row[4])
                    .GroupBy(row => (string)row[3])
                    .Select(group =>
                        new Grouping<IUriNode, Triple>(nodeGenerator.GetUri(group.Key),
                                group.Select(nodeGenerator.ConvertToTriple)));
            return graphs.Select(g => new Grouping<IUriNode, Triple>(g,
                gop.Search(g.ToString(), node2ComparableObject)
                    .Where(o => !(bool)((object[])o)[4])
                    .Select(o => ((object[])o))
                    .Select(nodeGenerator.ConvertToTriple)));
        }

        public IEnumerable<IGrouping<IUriNode, Triple>> GetTriplesFromGraphs(DataSet graphs)
        {
            if (!graphs.Any()) // all   todo
                return GetAllSubjects().SelectMany(node => GetTriplesWithSubjectFromGraphs(node, graphs));
                                
            return graphs.Select(g => new Grouping<IUriNode, Triple>(g,
                        gop.Search(g.ToString())
                                .Where(o => !(bool)((object[])o)[4])
                                .Select(o => ((object[])o))
                                .Select(nodeGenerator.ConvertToTriple)));
        }

     

        public DataSet GetGraphs(ISubjectNode sValue, IUriNode pValue, INode oValue, DataSet graphs)
        {
            var gs = spog.Search(sValue.ToString(), pValue.ToString(),
                nodeGenerator.Node2ComparableObject(oValue))
                .Where(o => !(bool)((object[])o)[4])
                .Select(o => ((object[])o)[3])
                .Cast<string>()
                .Select(nodeGenerator.GetUri);

            if (!graphs.Any())
                return new DataSet(gs);
            var s = new HashSet<IUriNode>(gs);
            s.IntersectWith(graphs);
            return new DataSet(s);
        }

        public IEnumerable<KeyValuePair<IUriNode, long>> GetAllGraphCounts()
        {
          var graphCount=new Dictionary<string, long>();
            spogdTable.Root.Scan(o =>
            {
                string g = (string) ((object[])o)[3];
                if (graphCount.ContainsKey(g)) graphCount[g]++;
                else graphCount.Add(g,1);
                return true;
            });
            return
                graphCount.Select(pair => new KeyValuePair<IUriNode, long>(nodeGenerator.GetUri(pair.Key), pair.Value));
        }

        public bool Contains(ISubjectNode sValue, IUriNode pValue, INode oValue, DataSet graphs)
        {
            return graphs.Any(graph => spog.Search(sValue.ToString(), pValue.ToString(),
                nodeGenerator.Node2ComparableObject(oValue), graph.ToString())
                .Any(o => !(bool) ((object[]) o)[4]));
        }

        public IGraph GetGraph(IUriNode graphUriNode)
        {
            return new QuadGraph(graphUriNode, this);
        }
          public IEnumerable<ISubjectNode> GetAllSubjects(IUriNode g)
          {
              return
                  gop.Search(g.ToString())
                      .Select(o => (object[]) o)
                      .Where(row => !(bool) row[4])
                      .Select(row => nodeGenerator.GetUri((string) row[0]));
          }
        #endregion

        #region edit named graphs
        public void ClearAllNamedGraphs()
        {
            foreach (var rowEntry in spogdTable.Root.Elements()
                .Where(rowEntry => !(rowEntry.Field(3).Get()).Equals(graphUri))
                .Where(rowEntry => !(bool) rowEntry.Field(4).Get()))
            {
                rowEntry.Field(4).Set(true);
                deletes++;
            }
            RemoveDeletedFromTable();
        }

        public void ClearAll()
        {
            spogdTable.Clear();
            spogdTable.Fill(new object[0]);
            nodeGenerator.Clear();
          //  nodeGenerator = new CodedNodeGenerator(path);
            spog.Clear();
            gop.Clear();
            osg.Clear();
            pgs.Clear();
            po.Clear();
            sg.Clear();
            deletes=0;
            base.graphUri = nodeGenerator.CreateUriNode(new UriPrefixed("dg:", "", storeSettings.Default.DefualtGraph));
        }
        public bool ContainsGraph(IUriNode g)
        {
           return gop.Search(g.ToString())
               .Any(o => !(bool) ((object[])o)[4]);
        }

      

        public IGraph CreateGraph(IUriNode g)
        {
            return GetGraph(g);
        }

        public void DropGraph(IUriNode g)
        {          
            Clear(g);
        }

        public void Clear(IUriNode g)
        {
            PaEntry lnk = spogdTable.Root.Element(0);
            foreach (var rowEntry in gop.SearchOffsets(g.ToString())
                .Select(offset =>
                {
                    lnk.offset = offset;
                    return lnk;
                })
                .Where(rowEntry => !(bool) rowEntry.Field(4).Get()))
            {
                rowEntry.Field(4).Set(true);
                deletes++;
            }
            RemoveDeletedFromTable();
        }

        public void Delete(IUriNode graph, IEnumerable<Triple> triples)
        {
            if (spogdTable.Root.IsEmpty) return;
            var lnk = spogdTable.Root.Element(0);
            foreach (var row in triples.SelectMany(triple => 
                gop.SearchOffsets(graph.ToString(), nodeGenerator.Node2ComparableObject(triple.Object), triple.Predicate.ToString())
                .Select(o =>
                {
                    lnk.offset = o;
                    return lnk;
                })
                .Where(row => ((string)row.Field(0).Get()).Equals(triple.Subject.ToString()) && !(bool) row.Field(4).Get())))
            {
                row.Field(4).Set(true);
                deletes++;
            }     
                RemoveDeletedFromTable();
        }

        public void Insert(IUriNode graph, IEnumerable<Triple> triples)
        {
            foreach (var triple in triples)
            {
                var row = new object[] { triple.Subject.ToString(), triple.Predicate.ToString(), nodeGenerator.Node2WritableObject(triple.Object), graphUri.ToString(), false };
                long offset = spogdTable.Root.AppendElement(row);
                //spog.Add((string) row[0], (string) row[1], nodeGenerator.Node2ComparableObject(triple.Object), (string) row[3], offset);
                //gop.Add((string) row[3], nodeGenerator.Node2ComparableObject(triple.Object), (string) row[1], offset);
                //pgs.Add((string) row[1], (string) row[3], (string) row[0], offset);
                //osg.Add(nodeGenerator.Node2ComparableObject(triple.Object), (string) row[0], (string) row[3], offset);
                //po.Add((string) row[1], nodeGenerator.Node2ComparableObject(triple.Object), offset);
                //sg.Add((string) row[0], (string) row[3], offset);    
            }         
        }


        public void AddGraph(IUriNode to, IGraph fromGraph)
        {
            Insert(to,fromGraph.GetTriples());
        }

        public void ReplaceGraph(IUriNode to, IGraph graph)
        {
           AddGraph(to, graph);
            Delete(to,graph.GetTriples());
        } 
        #endregion

    

        #endregion

        public bool Any(IUriNode uriNode)
        {
            return gop.Search(uriNode.ToString()).Any(o => !(bool) ((object[])o)[4]);
        }

    

        public void Build()
        {
            spog.Build();
            gop.Build();
            osg.Build();
            pgs.Build();
            po.Build();
            sg.Build();
            nodeGenerator.Build(); 
        }

        public void Close()
        {
          
            spogdTable.Close();
            nodeGenerator.Close();
            spog.Close();
            gop.Close();
            osg.Close();
            pgs.Close();
            po.Close();
            sg.Close();
        }

        public void Warm()
        {
            foreach (var element in spogdTable.Root.Elements());
            nodeGenerator.Warm();
            spog.Warm();
            gop.Warm();
            osg.Warm();
            pgs.Warm();
            po.Warm();
            sg.Warm();
        }
    }

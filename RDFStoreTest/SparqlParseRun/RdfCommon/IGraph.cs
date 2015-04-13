using System.Collections.Generic;

namespace SparqlParseRun.RdfCommon
{
    public interface IGraph
    {
        string Name { get; }

        INodeGenerator NodeGenerator { get; }   
      
        void Clear();
      //  void Build(); // Это действие отсутствует в стандарте dotnetrdf!


    /// <summary>
        /// Selects all Triples where the Object is a given Node
        /// </summary>
        /// <param name="n">Node</param>
        /// <returns></returns>
        IEnumerable<Triple> GetTriplesWithObject(INode n);

        /// <summary>
        /// Selects all Triples where the Predicate is a given Node
        /// </summary>
        /// <param name="n">Node</param>
        /// <returns></returns>
        IEnumerable<Triple> GetTriplesWithPredicate(IUriNode n);

        
        /// <summary>
        /// Selects all Triples where the Subject is a given Node
        /// </summary>
        /// <param name="n">Node</param>
        /// <returns></returns>
        IEnumerable<Triple> GetTriplesWithSubject(ISubjectNode n);

        /// <summary>
        /// Selects all Triples with the given Subject and Predicate
        /// </summary>
        /// <param name="subj">Subject</param>
        /// <param name="pred">Predicate</param>
        /// <returns></returns>
        IEnumerable<INode> GetTriplesWithSubjectPredicate(ISubjectNode subj, IUriNode pred);

        /// <summary>
        /// Selects all Triples with the given Subject and Object
        /// </summary>
        /// <param name="subj">Subject</param>
        /// <param name="obj">Object</param>
        /// <returns></returns>
        IEnumerable<IUriNode> GetTriplesWithSubjectObject(ISubjectNode subj, INode obj);

        /// <summary>
        /// Selects all Triples with the given Predicate and Object
        /// </summary>
        /// <param name="pred">Predicate</param>
        /// <param name="obj">Object</param>
        /// <returns></returns>
        IEnumerable<ISubjectNode> GetTriplesWithPredicateObject(IUriNode pred, INode obj);
        IEnumerable<Triple> GetTriples();


        void Add(ISubjectNode s, IUriNode p, INode o);
     
       // void LoadFrom(IUriNode @from);

        void Insert(IEnumerable<Triple> triples);

        void Add(Triple t);
        bool Contains(ISubjectNode subject, IUriNode predicate, INode node);
        void Delete(IEnumerable<Triple> triples);
      
        IEnumerable<ISubjectNode> GetAllSubjects();
        long GetTriplesCount();

        bool Any();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.IO;


namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		Base calculator class used to raise calculator and 
    ///             analyzer events in one consistent manner. Developers 
    ///             can override the protected members in base classes.
    ///Author:		www.devtreks.org
    ///Date:		2015, February
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. This class implements the Event-based Asynchronous Pattern.         
    /// </summary>
    public class BaseCalculator
    {
        // Good design
        private void Form1_RunCalculatorCompleted(object sender, RunCalculatorCompletedEventArgs e)
        {
            bool result = e.HasCalculations;
        }


        /////////////////////////////////////////////////////////////
        #region BaseCalculatorAsycn Implementation

        public delegate void ProgressChangedEventHandler(
            ProgressChangedEventArgs e);

        public delegate void RunCalculatorCompletedEventHandler(
            object sender,
            RunCalculatorCompletedEventArgs e);
        public delegate void SetAncestorsCompletedEventHandler(
            object sender,
            RunCalculatorCompletedEventArgs e);
        public delegate void RunDevPackCalculationCompletedEventHandler(
            object sender,
           RunCalculatorCompletedEventArgs e);

        // This class implements the Event-based Asynchronous Pattern. 
        // It asynchronously runs calculators and analyzers
        public class BaseCalculatorAsycn : Component
        {
            private delegate void WorkerEventHandler(
                XElement linkedViewElementToCalc,
                AsyncOperation asyncOp);

            private SendOrPostCallback onProgressReportDelegate;
            private SendOrPostCallback onCompletedDelegate;

            private HybridDictionary userStateToLifetime =
                new HybridDictionary();

            private System.ComponentModel.Container components = null;

            public CalculatorParameters GCCalculatorParams { get; set; }

            /////////////////////////////////////////////////////////////
            #region Public events

            public event ProgressChangedEventHandler ProgressChanged;
            public event RunCalculatorCompletedEventHandler RunCalculatorCompleted;
            public event SetAncestorsCompletedEventHandler SetAncestorsCompleted;
            public event RunDevPackCalculationCompletedEventHandler RunDevPackCalculationCompleted;


            #endregion

            /////////////////////////////////////////////////////////////
            #region Construction and destruction

            public BaseCalculatorAsycn(IContainer container, CalculatorParameters calcParameters)
            {
                this.GCCalculatorParams = new CalculatorParameters(calcParameters);

                container.Add(this);
                InitializeComponent();

                InitializeDelegates();
            }

            public BaseCalculatorAsycn(CalculatorParameters calcParameters)
            {
                this.GCCalculatorParams = new CalculatorParameters(calcParameters);
                InitializeComponent();

                InitializeDelegates();
            }

            protected virtual void InitializeDelegates()
            {
                onProgressReportDelegate =
                    new SendOrPostCallback(ReportProgress);
                onCompletedDelegate =
                    new SendOrPostCallback(CalculateCompleted);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
                base.Dispose(disposing);
            }

            #endregion // Construction and destruction

            ///////////////////////////////////////////////////////////// 
            ///
            #region Implementation

            // This method starts an asynchronous calculation.  
            // First, it checks the supplied task ID for uniqueness. 
            // If taskId is unique, it creates a new WorkerEventHandler  
            // and calls its BeginInvoke method to start the calculation. 
            public virtual void RunCalculatorAsync(
                XElement currentElement,
                object taskId)
            {
                // Create an AsyncOperation for taskId.
                AsyncOperation asyncOp =
                    AsyncOperationManager.CreateOperation(taskId);

                // Multiple threads will access the task dictionary, 
                // so it must be locked to serialize access. 
                lock (userStateToLifetime.SyncRoot)
                {
                    if (userStateToLifetime.Contains(taskId))
                    {
                        throw new ArgumentException(
                            "Task ID parameter must be unique",
                            "taskId");
                    }

                    userStateToLifetime[taskId] = asyncOp;
                }

                // Start the asynchronous operation.
                WorkerEventHandler workerDelegate = new WorkerEventHandler(CalculateWorker);
                workerDelegate.BeginInvoke(
                    currentElement,
                    asyncOp,
                    null,
                    null);
            }

            // Utility method for determining if a  
            // task has been canceled. 
            private bool TaskCanceled(object taskId)
            {
                return (userStateToLifetime[taskId] == null);
            }

            // This method cancels a pending asynchronous operation. 
            public void CancelAsync(object taskId)
            {
                AsyncOperation asyncOp = userStateToLifetime[taskId] as AsyncOperation;
                if (asyncOp != null)
                {
                    lock (userStateToLifetime.SyncRoot)
                    {
                        userStateToLifetime.Remove(taskId);
                    }
                }
            }

            //private void CalculateWorker(
            //    ref XElement currentElement,
            //    AsyncOperation asyncOp)
            //{
            //    bool bHasCalculations = false;
            //    if (this.GCCalculatorParams.RunCalculatorType
            //        == CalculatorHelpers.RUN_CALCULATOR_TYPES.basic)
            //    {
            //        bHasCalculations
            //            = RunBasicCalculationsAndSetUpdates(ref currentElement);
            //    }
            //    //else if (this.GCCalculatorParams.RunCalculatorType
            //    //    == CalculatorHelpers.RUN_CALCULATOR_TYPES.iotechnology)
            //    //{
            //    //    bHasCalculations
            //    //        = RunIOTechCalculationsAndSetUpdates(ref currentElement);
            //    //}
            //    //else if (this.GCCalculatorParams.RunCalculatorType
            //    //    == CalculatorHelpers.RUN_CALCULATOR_TYPES.calcdocuri)
            //    //{
            //    //    bHasCalculations
            //    //        = RunIOTechCalculationsAndSetUpdates(ref currentElement);
            //    //}
            //    //else if (this.GCCalculatorParams.RunCalculatorType
            //    //    == CalculatorHelpers.RUN_CALCULATOR_TYPES.analyzer)
            //    //{
            //    //    bHasCalculations
            //    //        = RunStatisticalCalculation(ref currentElement);
            //    //}
            //    //else if (this.GCCalculatorParams.RunCalculatorType
            //    //    == CalculatorHelpers.RUN_CALCULATOR_TYPES.analyzeobjects)
            //    //{
            //    //    //1.3.6
            //    //    bHasCalculations
            //    //        = RunBasicAnalysisAndSetUpdates(ref currentElement);
            //    //}
            //    //return bHasCalculations;
            //}
            


            // This method performs the actual prime number computation. 
            // It is executed on the worker thread. 
            private void CalculateWorker(
                XElement currentElement,
                AsyncOperation asyncOp)
            {
                bool hasCalculation = false;
                XElement linkedViewElement = XElement.Load(Constants.ROOT_PATH);
                Exception e = null;

                // Check that the task is still active. 
                // The operation may have been canceled before 
                // the thread was scheduled. 
                if (!TaskCanceled(asyncOp.UserSuppliedState))
                {
                    try
                    {
                        if (this.GCCalculatorParams.RunCalculatorType
                            == CalculatorHelpers.RUN_CALCULATOR_TYPES.basic)
                        {
                            //the refs are passed to the completion method below
                            hasCalculation
                                = RunBasicCalculationsAndSetUpdates(ref currentElement,
                                    ref linkedViewElement, asyncOp);
                        }
                        //// Find all the prime numbers up to  
                        //// the square root of linkedViewElementToCalc.
                        //ArrayList primes = BuildPrimeNumberList(
                        //    currentElement,
                        //    asyncOp);

                        //// Now we have a list of primes less than 
                        //// linkedViewElementToCalc.
                        //hasCalculation = IsPrime(
                        //    primes,
                        //    ref currentElement,
                        //    ref linkedViewElementToCalc);
                    }
                    catch (Exception ex)
                    {
                        e = ex;
                    }
                }

                //RunCalculatorState calcState = new RunCalculatorState( 
                //        linkedViewElementToCalc, 
                //        currentElementToCalc, 
                //        hasCalculation, 
                //        e, 
                //        TaskCanceled(asyncOp.UserSuppliedState), 
                //        asyncOp); 

                //this.CompletionMethod(calcState); 

                this.CompletionMethod(
                    currentElement,
                    linkedViewElement,
                    hasCalculation,
                    e,
                    TaskCanceled(asyncOp.UserSuppliedState),
                    asyncOp);

                //completionMethodDelegate(calcState);
            }
            //// This method performs the actual prime number computation. 
            //// It is executed on the worker thread. 
            //private void CalculateWorker(
            //    int numberToTest,
            //    AsyncOperation asyncOp)
            //{
            //    bool isPrime = false;
            //    int firstDivisor = 1;
            //    Exception e = null;

            //    // Check that the task is still active. 
            //    // The operation may have been canceled before 
            //    // the thread was scheduled. 
            //    if (!TaskCanceled(asyncOp.UserSuppliedState))
            //    {
            //        try
            //        {
            //            // Find all the prime numbers up to  
            //            // the square root of numberToTest.
            //            ArrayList primes = BuildPrimeNumberList(
            //                numberToTest,
            //                asyncOp);

            //            // Now we have a list of primes less than 
            //            // numberToTest.
            //            isPrime = IsPrime(
            //                primes,
            //                numberToTest,
            //                out firstDivisor);
            //        }
            //        catch (Exception ex)
            //        {
            //            e = ex;
            //        }
            //    }

            //    //CalculatePrimeState calcState = new CalculatePrimeState( 
            //    //        numberToTest, 
            //    //        firstDivisor, 
            //    //        isPrime, 
            //    //        e, 
            //    //        TaskCanceled(asyncOp.UserSuppliedState), 
            //    //        asyncOp); 

            //    //this.CompletionMethod(calcState); 

            //    this.CompletionMethod(
            //        numberToTest,
            //        firstDivisor,
            //        isPrime,
            //        e,
            //        TaskCanceled(asyncOp.UserSuppliedState),
            //        asyncOp);

            //    //completionMethodDelegate(calcState);
            //}
            private bool RunBasicCalculationsAndSetUpdates(ref XElement currentElement,
                ref XElement linkedViewElement, AsyncOperation asyncOp)
            {
                bool bHasCalculations = false;
                ProgressChangedEventArgs e = null;

                if (!currentElement.HasAttributes)
                    return true;
                // Do the work. 
                while (!TaskCanceled(asyncOp.UserSuppliedState))
                {
                    //1. set parameters needed by updates collection
                    this.GCCalculatorParams.CurrentElementNodeName
                        = currentElement.Name.LocalName;
                    this.GCCalculatorParams.CurrentElementURIPattern
                        = CalculatorHelpers.MakeNewURIPatternFromElement(this.GCCalculatorParams.ExtensionDocToCalcURI.URIPattern,
                        currentElement);
                    //2. don't run calcs on ancestors
                    bool bIsSelfOrDescendentNode = CalculatorHelpers.IsSelfOrDescendentNode(
                       this.GCCalculatorParams, this.GCCalculatorParams.CurrentElementNodeName);
                    if (bIsSelfOrDescendentNode)
                    {
                        //3. get the calculator to use 
                        //(this.GCCalculatorParams.CalculationEl, or currentElement.xmldoc)
                        linkedViewElement = null;
                        //XElement linkedViewElement = null;
                        linkedViewElement = CalculatorHelpers.GetCalculator(
                            this.GCCalculatorParams, currentElement);
                        //some apps, such as locals, work differently 
                        AdjustSpecialtyLinkedViewElements(currentElement, ref linkedViewElement);
                        //4. Set bool to update base node attributes in db
                        this.GCCalculatorParams.AttributeNeedsDbUpdate
                            = CalculatorHelpers.NeedsUpdateAttribute(this.GCCalculatorParams);
                        ////5. raise event to carry out calculations
                        //GCArguments.CurrentElement = currentElement;
                        //GCArguments.LinkedViewElement = linkedViewElement;
                        //OnRunCalculator(GCArguments);
                        //currentElement = GCArguments.CurrentElement;
                        //linkedViewElement = GCArguments.LinkedViewElement;
                        //bHasCalculations = GCArguments.HasCalculations;
                        if (bHasCalculations)
                        {
                            //6. 100% Rules: don't allow analyzers to db update descendent calculators
                            ChangeLinkedViewCalculator(currentElement, ref linkedViewElement);
                            //7. replace the this.GCCalculatorParams.LinkedViewElement when 
                            //the originating doctocalcuri node is processed
                            bool bHasReplacedCalculator = CalculatorHelpers.ReplaceCalculations(this.GCCalculatorParams,
                                currentElement, linkedViewElement);
                            //8. SetXmlDocAttributes
                            CalculatorHelpers.SetXmlDocUpdates(this.GCCalculatorParams,
                                linkedViewElement, ref currentElement,
                                this.GCCalculatorParams.Updates);
                        }
                    }
                    else
                    {
                        //basic calculators don't need full collections that include ancestors
                        bHasCalculations = true;
                    }
                    if (bHasCalculations)
                    {
                        //code left this var out - should be 100% complete here
                        int progressPercent = 100;
                        // Report to the client that a prime was found.
                        e = new RunCalculatorProgressChangedEventArgs(
                            currentElement,
                            linkedViewElement,
                            progressPercent,
                            asyncOp.UserSuppliedState);

                        asyncOp.Post(this.onProgressReportDelegate, e);

                        // Yield the rest of this time slice.
                        Thread.Sleep(0);
                    }
                }
                return bHasCalculations;
            }
            // This method computes the list of prime numbers used by the 
            // IsPrime method. 
            //private ArrayList BuildPrimeNumberList(
            //    XElement linkedViewElementToCalc,
            //    AsyncOperation asyncOp)
            //{
            //    ProgressChangedEventArgs e = null;
            //    ArrayList primes = new ArrayList();
            //    XElement currentElementToCalc;
            //    int n = 5;

            //    // Add the first prime numbers.
            //    primes.Add(2);
            //    primes.Add(3);

            //    // Do the work. 
            //    while (n < linkedViewElementToCalc &&
            //           !TaskCanceled(asyncOp.UserSuppliedState))
            //    {
            //        if (IsPrime(primes, n, out currentElementToCalc))
            //        {
            //            // Report to the client that a prime was found.
            //            e = new RunCalculatorProgressChangedEventArgs(
            //                n,
            //                (int)((float)n / (float)linkedViewElementToCalc * 100),
            //                asyncOp.UserSuppliedState);

            //            asyncOp.Post(this.onProgressReportDelegate, e);

            //            primes.Add(n);

            //            // Yield the rest of this time slice.
            //            Thread.Sleep(0);
            //        }

            //        // Skip even numbers.
            //        n += 2;
            //    }

            //    return primes;
            //}

            //// This method tests n for primality against the list of  
            //// prime numbers contained in the primes parameter. 
            //private bool IsPrime(
            //    ArrayList primes,
            //    ref XElement linkedViewElementToCalc,
            //    ref XElement currentElementToCalc)
            //{
            //    bool foundDivisor = false;
            //    bool exceedsSquareRoot = false;

            //    int i = 0;
            //    int divisor = 0;
            //    //currentElementToCalc = 1;

            //    // Stop the search if: 
            //    // there are no more primes in the list, 
            //    // there is a divisor of n in the list, or 
            //    // there is a prime that is larger than  
            //    // the square root of n. 
            //    while (
            //        (i < primes.Count) &&
            //        !foundDivisor &&
            //        !exceedsSquareRoot)
            //    {
            //        // The divisor variable will be the smallest  
            //        // prime number not yet tried.
            //        divisor = (int)primes[i++];

            //        // Determine whether the divisor is greater 
            //        // than the square root of n. 
            //        if (divisor * divisor > n)
            //        {
            //            exceedsSquareRoot = true;
            //        }
            //        // Determine whether the divisor is a factor of n. 
            //        else if (n % divisor == 0)
            //        {
            //            currentElementToCalc = divisor;
            //            foundDivisor = true;
            //        }
            //    }

            //    return !foundDivisor;
            //}

            // This method is invoked via the AsyncOperation object, 
            // so it is guaranteed to be executed on the correct thread. 
            private void CalculateCompleted(object operationState)
            {
                RunCalculatorCompletedEventArgs e =
                    operationState as RunCalculatorCompletedEventArgs;

                OnRunCalculatorCompleted(e);
            }

            // This method is invoked via the AsyncOperation object, 
            // so it is guaranteed to be executed on the correct thread. 
            private void ReportProgress(object state)
            {
                ProgressChangedEventArgs e =
                    state as ProgressChangedEventArgs;

                OnProgressChanged(e);
            }

            protected void OnRunCalculatorCompleted(
                RunCalculatorCompletedEventArgs e)
            {
                if (RunCalculatorCompleted != null)
                {
                    RunCalculatorCompleted(this, e);
                }
            }

            protected void OnProgressChanged(ProgressChangedEventArgs e)
            {
                if (ProgressChanged != null)
                {
                    ProgressChanged(e);
                }
            }

            // This is the method that the underlying, free-threaded  
            // asynchronous behavior will invoke.  This will happen on 
            // an arbitrary thread. 
            private void CompletionMethod(
                XElement currentElementToCalc,
                XElement linkedViewElementToCalc,
                bool hasCalculation,
                Exception exception,
                bool canceled,
                AsyncOperation asyncOp)
            {
                // If the task was not previously canceled, 
                // remove the task from the lifetime collection. 
                if (!canceled)
                {
                    lock (userStateToLifetime.SyncRoot)
                    {
                        userStateToLifetime.Remove(asyncOp.UserSuppliedState);
                    }
                }

                // Package the results of the operation in a  
                // RunCalculatorCompletedEventArgs.
                RunCalculatorCompletedEventArgs e =
                    new RunCalculatorCompletedEventArgs(this.GCCalculatorParams,
                    currentElementToCalc,
                    linkedViewElementToCalc,
                    hasCalculation,
                    exception,
                    canceled,
                    asyncOp.UserSuppliedState);

                // End the task. The asyncOp object is responsible  
                // for marshaling the call.
                asyncOp.PostOperationCompleted(onCompletedDelegate, e);

                // Note that after the call to OperationCompleted,  
                // asyncOp is no longer usable, and any attempt to use it 
                // will cause an exception to be thrown.
            }
            private void AdjustSpecialtyLinkedViewElements(XElement currentElement,
            ref XElement linkedViewElement)
            {
                if (this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.SubAppType
                    == Constants.SUBAPPLICATION_TYPES.locals.ToString())
                {
                    if (currentElement.Name.LocalName
                        == Local.LOCAL_TYPES.localaccountgroup.ToString())
                    {
                        linkedViewElement = null;
                    }
                    else if (currentElement.Name.LocalName
                        == Local.LOCAL_TYPES.local.ToString())
                    {
                        //local nodes are treated the same as children of starting group node
                        this.GCCalculatorParams.NeedsCalculators = true;
                        //locals have already been inserted
                        this.GCCalculatorParams.NeedsXmlDocOnly = true;
                        linkedViewElement = new XElement(this.GCCalculatorParams.LinkedViewElement);
                    }
                }
            }
            private void ChangeLinkedViewCalculator(XElement currentElement, ref XElement linkedViewElement)
            {
                //v137 pattern allows analyzers to update descendents using dbupdates
                //i.e. need i/o calculators to get totals, but don't want to 
                //overwrite those calculations in db
                if (this.GCCalculatorParams.ExtensionCalcDocURI.URIDataManager.HostName
                    == DevTreks.Data.Helpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString()
                    && this.GCCalculatorParams.NeedsCalculators
                    && CalculatorHelpers.IsSelfOrChildNode(this.GCCalculatorParams, currentElement.Name.LocalName))
                {
                    //100% Rule 1: Analyzers never, ever, update calculators
                    string sCalculatorType = CalculatorHelpers.GetAttribute(linkedViewElement,
                        Calculator1.cCalculatorType);
                    //pure calculators never have an analysis type
                    string sAnalysisType = CalculatorHelpers.GetAttribute(linkedViewElement,
                        Calculator1.cAnalyzerType);
                    if (!string.IsNullOrEmpty(sCalculatorType)
                        && string.IsNullOrEmpty(sAnalysisType))
                    {
                        //order of lv retrieval gets calulators before analyzers
                        XElement analyzerLV = CalculatorHelpers.GetChildLinkedViewUsingAttribute(
                            currentElement, Calculator1.cAnalyzerType, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                        if (analyzerLV != null)
                        {
                            if (this.GCCalculatorParams.LinkedViewElement != null)
                            {
                                //keep the id and calculatorid, but update the rest of the atts with new lv
                                string sId = CalculatorHelpers.GetAttribute(analyzerLV, Calculator1.cId);
                                string sCalcId = CalculatorHelpers.GetAttribute(analyzerLV, Calculator1.cCalculatorId);
                                analyzerLV = new XElement(this.GCCalculatorParams.LinkedViewElement);
                                CalculatorHelpers.SetAttribute(analyzerLV, Calculator1.cId, sId);
                                CalculatorHelpers.SetAttribute(analyzerLV, Calculator1.cCalculatorId, sCalcId);
                            }
                            //use it to update db (not the calculator)
                            linkedViewElement = new XElement(analyzerLV);
                        }
                        else
                        {
                            //use the base linked view standard pattern
                            //avoids updating the wrong lvs
                            linkedViewElement = CalculatorHelpers.GetNewCalculator(this.GCCalculatorParams, currentElement);
                        }
                    }
                }
                //100% Rule 2: Analyzers and Calculators never, ever, allow descendent lvs 
                //to have parent Overwrite or UseSameCalc properties
                if (this.GCCalculatorParams.StartingDocToCalcNodeName
                    != currentElement.Name.LocalName)
                {
                    if (linkedViewElement != null)
                    {
                        string sAttValue = CalculatorHelpers.GetAttribute(linkedViewElement,
                            Calculator1.cUseSameCalculator);
                        if ((!string.IsNullOrEmpty(sAttValue)) && sAttValue != Constants.NONE)
                        {
                            CalculatorHelpers.SetAttribute(linkedViewElement,
                                Calculator1.cUseSameCalculator, string.Empty);
                        }
                        sAttValue = CalculatorHelpers.GetAttribute(linkedViewElement,
                            Calculator1.cOverwrite);
                        if ((!string.IsNullOrEmpty(sAttValue)) && sAttValue != Constants.NONE)
                        {
                            CalculatorHelpers.SetAttribute(linkedViewElement,
                                Calculator1.cOverwrite, string.Empty);
                        }
                    }
                }
            }
            #endregion

            /////////////////////////////////////////////////////////////
            #region Component Designer generated code

            private void InitializeComponent()
            {
                components = new System.ComponentModel.Container();
            }

            #endregion

        }

        public class RunCalculatorProgressChangedEventArgs :
                ProgressChangedEventArgs
        {
            private XElement latestCurrentElement = XElement.Load(Constants.ROOT_PATH);

            public RunCalculatorProgressChangedEventArgs(
                XElement currentElement,
                XElement linkedViewElement,
                int progressPercentage,
                object userToken)
                : base(progressPercentage, userToken)
            {
                this.latestCurrentElement = currentElement;
            }
            //includes updated linkedview holding calcs
            public XElement LatestCurrentElement
            {
                get
                {
                    return latestCurrentElement;
                }
            }
        }

        public class RunCalculatorCompletedEventArgs :
            AsyncCompletedEventArgs
        {
            //must be read only
            //returns true if calcs run successfully
            private bool hasCalculation = false;
            //documentation said these must be read only, but example includes write too
            private CalculatorParameters calculatorParams = new CalculatorParameters();
            private XElement linkedViewElementToCalc;
            private XElement currentElementToCalc;
            private IEnumerable<XElement> linkedViewElements;


            //private int numberToTest = 0;

            public RunCalculatorCompletedEventArgs(CalculatorParameters calcorParams,
                XElement currentElementToCalc,
                XElement linkedViewElementToCalc,
                bool hasCalculation,
                Exception e,
                bool canceled,
                object state)
                : base(e, canceled, state)
            {
                this.calculatorParams = calcorParams;
                this.linkedViewElementToCalc = linkedViewElementToCalc;
                this.currentElementToCalc = currentElementToCalc;
                this.hasCalculation = hasCalculation;
            }
            public CalculatorParameters CalculatorParams
            {
                get
                {
                    // Raise an exception if the operation failed or  
                    // was canceled.
                    RaiseExceptionIfNecessary();

                    // If the operation was successful, return params
                    return calculatorParams;
                }
            }
            public XElement LinkedViewElement
            {
                get
                {
                    // Raise an exception if the operation failed or  
                    // was canceled.
                    RaiseExceptionIfNecessary();

                    // If the operation was successful, return the  
                    // property value. 
                    return linkedViewElementToCalc;
                }
            }

            public XElement CurrentElement
            {
                get
                {
                    // Raise an exception if the operation failed or  
                    // was canceled.
                    RaiseExceptionIfNecessary();

                    // If the operation was successful, return the  
                    // property value. 
                    return currentElementToCalc;
                }
            }

            public bool HasCalculations
            {
                get
                {
                    // Raise an exception if the operation failed or  
                    // was canceled.
                    RaiseExceptionIfNecessary();

                    // If the operation was successful, return true 
                    return hasCalculation;
                }
            }
            //these elements contain all children linkedviews
            //subscriber chooses which one is needed
            public IEnumerable<XElement> LinkedViewElements
            {
                get
                {
                    // Raise an exception if the operation failed or  
                    // was canceled.
                    RaiseExceptionIfNecessary();

                    // If the operation was successful, return the property value
                    return linkedViewElements;
                }
            }
        }

        
        #endregion








        ////allows derived classes to override the event invocation behavior
        //protected virtual async Task RunCalculatorAsync(CustomEventArgs e)
        //{
        //    //make a temporary copy of the event to avoid possibility of
        //    //a race condition if the last subscriber unsubscribes
        //    //immediately after the null check and before the event is raised.
        //    RunCalculatorCompletedEventHandler handler = RunCalculatorCompleted;

        //    //event will be null if there are no subscribers
        //    if (handler != null)
        //    {
        //        //prepare the arguments to send inside the CustomEventArgs parameter
        //        e.CalculatorParams = this.GCCalculatorParams;
        //        //use the () operator to raise the event.
        //        handler(this, e);
        //    }
        //}

        ////allows derived classes to override the event invocation behavior
        //protected virtual async Task RunCalculatorCancelledAsync(CustomEventArgs e)
        //{
        //    //make a temporary copy of the event to avoid possibility of
        //    //a race condition if the last subscriber unsubscribes
        //    //immediately after the null check and before the event is raised.
        //    EventHandler<CustomEventArgs> handler = RunCalculatorCancelled;

        //    //event will be null if there are no subscribers
        //    if (handler != null)
        //    {
        //        //prepare the arguments to send inside the CustomEventArgs parameter
        //        e.CalculatorParams = this.GCCalculatorParams;
        //        //use the () operator to raise the event.
        //        handler(this, e);
        //    }
        //}
        ////allows derived classes to override the event invocation behavior
        //protected virtual async Task RunCalculatorProgressChangedAsync(CustomEventArgs e)
        //{
        //    //make a temporary copy of the event to avoid possibility of
        //    //a race condition if the last subscriber unsubscribes
        //    //immediately after the null check and before the event is raised.
        //    EventHandler<CustomEventArgs> handler = RunCalculatorProgressChanged;

        //    //event will be null if there are no subscribers
        //    if (handler != null)
        //    {
        //        //prepare the arguments to send inside the CustomEventArgs parameter
        //        e.CalculatorParams = this.GCCalculatorParams;
        //        //use the () operator to raise the event.
        //        handler(this, e);
        //    }
        //}
        ////allows derived classes to override the event invocation behavior
        //protected virtual async Task SetAncestorsAsync(CustomEventArgs e)
        //{
        //    //make a temporary copy of the event to avoid possibility of
        //    //a race condition if the last subscriber unsubscribes
        //    //immediately after the null check and before the event is raised.
        //    EventHandler<CustomEventArgs> handler = SetAncestorsCompleted;

        //    //event will be null if there are no subscribers
        //    if (handler != null)
        //    {
        //        //prepare the arguments to send inside the CustomEventArgs parameter
        //        e.CalculatorParams = this.GCCalculatorParams;
        //        //use the () operator to raise the event.
        //        handler(this, e);
        //    }
        //}
        ////allows derived classes to override the event invocation behavior
        //protected virtual async Task RunDevPackCalculationsAsync(CustomEventArgs e)
        //{
        //    //make a temporary copy of the event to avoid possibility of
        //    //a race condition if the last subscriber unsubscribes
        //    //immediately after the null check and before the event is raised.
        //    EventHandler<CustomEventArgs> handler = RunDevPackCalculationCompleted;

        //    //event will be null if there are no subscribers
        //    if (handler != null)
        //    {
        //        //prepare the arguments to send inside the CustomEventArgs parameter
        //        //use the () operator to raise the event.
        //        handler(this, e);
        //    }
        //}

        //public async Task<bool> RunCalculatorsAndSetUpdatesAsync(XElement currentElement)
        //{
        //    bool bHasCalculations = false;
        //    if (this.GCCalculatorParams.RunCalculatorType
        //        == CalculatorHelpers.RUN_CALCULATOR_TYPES.basic)
        //    {
        //        bHasCalculations
        //            = await RunBasicCalculationsAndSetUpdatesAsync(currentElement);
        //    }
        //    else if (this.GCCalculatorParams.RunCalculatorType
        //        == CalculatorHelpers.RUN_CALCULATOR_TYPES.iotechnology)
        //    {
        //        bHasCalculations
        //            = await RunIOTechCalculationsAndSetUpdatesAsync(currentElement);
        //    }
        //    else if (this.GCCalculatorParams.RunCalculatorType
        //        == CalculatorHelpers.RUN_CALCULATOR_TYPES.calcdocuri)
        //    {
        //        bHasCalculations
        //            = await RunIOTechCalculationsAndSetUpdatesAsync(currentElement);
        //    }
        //    else if (this.GCCalculatorParams.RunCalculatorType
        //        == CalculatorHelpers.RUN_CALCULATOR_TYPES.analyzer)
        //    {
        //        bHasCalculations
        //            = await RunStatisticalCalculationAsync(currentElement);
        //    }
        //    else if (this.GCCalculatorParams.RunCalculatorType
        //        == CalculatorHelpers.RUN_CALCULATOR_TYPES.analyzeobjects)
        //    {
        //        //1.3.6
        //        bHasCalculations
        //            = await RunBasicAnalysisAndSetUpdatesAsync(currentElement);
        //    }
        //    return bHasCalculations;
        //}
        //private async Task<bool> RunBasicCalculationsAndSetUpdatesAsync(XElement currentElement)
        //{
        //    bool bHasCalculations = false;
        //    if (!currentElement.HasAttributes)
        //        return true;
        //    //1. set parameters needed by updates collection
        //    this.GCCalculatorParams.CurrentElementNodeName
        //        = currentElement.Name.LocalName;
        //    this.GCCalculatorParams.CurrentElementURIPattern
        //        = CalculatorHelpers.MakeNewURIPatternFromElement(this.GCCalculatorParams.ExtensionDocToCalcURI.URIPattern,
        //        currentElement);
        //    //2. don't run calcs on ancestors
        //    bool bIsSelfOrDescendentNode = CalculatorHelpers.IsSelfOrDescendentNode(
        //       this.GCCalculatorParams, this.GCCalculatorParams.CurrentElementNodeName);
        //    if (bIsSelfOrDescendentNode)
        //    {
        //        //3. get the calculator to use 
        //        //(this.GCCalculatorParams.CalculationEl, or currentElement.xmldoc)
        //        XElement linkedViewElement = null;
        //        linkedViewElement = CalculatorHelpers.GetCalculator(
        //            this.GCCalculatorParams, currentElement);
        //        //some apps, such as locals, work differently 
        //        AdjustSpecialtyLinkedViewElements(currentElement, linkedViewElement);
        //        //4. Set bool to update base node attributes in db
        //        this.GCCalculatorParams.AttributeNeedsDbUpdate
        //            = CalculatorHelpers.NeedsUpdateAttribute(this.GCCalculatorParams);
        //        //5. raise event to carry out calculations
        //        GCArguments.CurrentElement = currentElement;
        //        GCArguments.LinkedViewElement = linkedViewElement;
        //        await RunCalculatorAsync(GCArguments);
        //        currentElement = GCArguments.CurrentElement;
        //        linkedViewElement = GCArguments.LinkedViewElement;
        //        bHasCalculations = GCArguments.HasCalculations;
        //        if (bHasCalculations)
        //        {
        //            //6. 100% Rules: don't allow analyzers to db update descendent calculators
        //            ChangeLinkedViewCalculator(currentElement, linkedViewElement);
        //            //7. replace the this.GCCalculatorParams.LinkedViewElement when 
        //            //the originating doctocalcuri node is processed
        //            bool bHasReplacedCalculator = CalculatorHelpers.ReplaceCalculations(this.GCCalculatorParams,
        //                currentElement, linkedViewElement);
        //            //8. SetXmlDocAttributes
        //            CalculatorHelpers.SetXmlDocUpdates(this.GCCalculatorParams,
        //                linkedViewElement, ref currentElement,
        //                this.GCCalculatorParams.Updates);
        //        }
        //    }
        //    else
        //    {
        //        //basic calculators don't need full collections that include ancestors
        //        bHasCalculations = true;
        //    }
        //    return bHasCalculations;
        //}
        //private async Task<bool> RunBasicAnalysisAndSetUpdatesAsync(XElement currentElement)
        //{
        //    bool bHasCalculations = false;
        //    if (!currentElement.HasAttributes)
        //        return true;
        //    //1. set parameters needed by updates collection
        //    this.GCCalculatorParams.CurrentElementNodeName
        //        = currentElement.Name.LocalName;
        //    this.GCCalculatorParams.CurrentElementURIPattern
        //        = CalculatorHelpers.MakeNewURIPatternFromElement(this.GCCalculatorParams.ExtensionDocToCalcURI.URIPattern,
        //        currentElement);
        //    //2. don't run calcs on ancestors
        //    bool bIsSelfOrDescendentNode = CalculatorHelpers.IsSelfOrDescendentNode(
        //       this.GCCalculatorParams, this.GCCalculatorParams.CurrentElementNodeName);
        //    if (bIsSelfOrDescendentNode)
        //    {
        //        //3. get the calculator to use 
        //        //(this.GCCalculatorParams.CalculationEl, or currentElement.xmldoc)
        //        XElement linkedViewElement = null;
        //        linkedViewElement = CalculatorHelpers.GetCalculator(
        //            this.GCCalculatorParams, currentElement);
        //        //4. Set bool to update base node attributes in db
        //        this.GCCalculatorParams.AttributeNeedsDbUpdate
        //            = CalculatorHelpers.NeedsUpdateAttribute(this.GCCalculatorParams);
        //        //5. raise event to carry out calculations
        //        GCArguments.CurrentElement = currentElement;
        //        GCArguments.LinkedViewElement = linkedViewElement;
        //        await RunCalculatorAsync(GCArguments);
        //        currentElement = GCArguments.CurrentElement;
        //        //subscriber sets the lv to update 
        //        linkedViewElement = GCArguments.LinkedViewElement;
        //        bHasCalculations = GCArguments.HasCalculations;
        //        if (bHasCalculations)
        //        {
        //            //6. allow analyzers to db update descendent analyzers
        //            ChangeLinkedViewCalculatorForAnalysis(currentElement, linkedViewElement);
        //            //7. replace the this.GCCalculatorParams.LinkedViewElement when 
        //            //the originating doctocalcuri node is processed
        //            bool bHasReplacedCalculator = CalculatorHelpers.ReplaceCalculations(this.GCCalculatorParams,
        //                currentElement, linkedViewElement);
        //            //8. SetXmlDocAttributes
        //            CalculatorHelpers.SetXmlDocUpdates(this.GCCalculatorParams,
        //                linkedViewElement, ref currentElement,
        //                this.GCCalculatorParams.Updates);
        //        }
        //    }
        //    else
        //    {
        //        //version 1.3.6 added this so that ancestors are always added to collections
        //        //to make analyses consistent (i.e. always start with group)
        //        GCArguments.CurrentElement = currentElement;
        //        //ancestors can never update or use a linkedview element
        //        GCArguments.LinkedViewElement = null;
        //        //but we want the ancestor current element added to the underlying collections
        //        await RunCalculatorAsync(GCArguments);
        //        //always return true, so no error msg is generated
        //        bHasCalculations = true;
        //    }
        //    return bHasCalculations;
        //}
        //private async Task<bool> RunIOTechCalculationsAndSetUpdatesAsync(XElement currentElement)
        //{
        //    bool bHasCalculations = false;
        //    if (!currentElement.HasAttributes)
        //        return true;
        //    //1. set parameters needed by updates collection
        //    this.GCCalculatorParams.CurrentElementNodeName
        //        = currentElement.Name.LocalName;
        //    this.GCCalculatorParams.CurrentElementURIPattern
        //        = CalculatorHelpers.MakeNewURIPatternFromElement(this.GCCalculatorParams.ExtensionDocToCalcURI.URIPattern,
        //        currentElement);
        //    //2. don't run calcs on ancestors
        //    bool bIsSelfOrDescendentNode = CalculatorHelpers.IsSelfOrDescendentNode(
        //       this.GCCalculatorParams, this.GCCalculatorParams.CurrentElementNodeName);
        //    if (bIsSelfOrDescendentNode)
        //    {
        //        //3. get the calculator to use 
        //        //(this.GCCalculatorParams.CalculationEl, or currentElement.xmldoc)
        //        XElement linkedViewElement = null;
        //        bool bNeedsRelatedCalculator = NeedsRelatedCalculator();
        //        if (bNeedsRelatedCalculator)
        //        {
        //            //no db updates -potential to continually have to update in/outs in db
        //            linkedViewElement
        //                = CalculatorHelpers.GetRelatedCalculator(this.GCCalculatorParams,
        //                     currentElement);
        //        }
        //        else
        //        {
        //            //don't use calctype = input; prefer using stronger relatedcalctype=agmachinery
        //            string sCalculatorType = this.GCCalculatorParams.CalculatorType;
        //            this.GCCalculatorParams.CalculatorType = string.Empty;
        //            linkedViewElement
        //               = CalculatorHelpers.GetCalculator(this.GCCalculatorParams,
        //                    currentElement);
        //            this.GCCalculatorParams.CalculatorType = sCalculatorType;
        //        }
        //        //4. Set bool to update base node attributes in db
        //        this.GCCalculatorParams.AttributeNeedsDbUpdate
        //            = CalculatorHelpers.NeedsUpdateAttribute(this.GCCalculatorParams);
        //        //5. raise event to carry out calculations
        //        GCArguments.CurrentElement = currentElement;
        //        GCArguments.LinkedViewElement = linkedViewElement;
        //        await RunCalculatorAsync(GCArguments);
        //        currentElement = GCArguments.CurrentElement;
        //        linkedViewElement = GCArguments.LinkedViewElement;
        //        bHasCalculations = GCArguments.HasCalculations;
        //        //6. 100% Rules: don't allow analyzers to db update descendent calculators
        //        ChangeLinkedViewCalculator(currentElement, linkedViewElement);
        //        //inputs have a join-side xmldoc that may need to be updated 
        //        //to current calculator results (i.e. Input.Times can change
        //        //the amount of fuel, labor, and other resources for this input)
        //        //note that the base npvcalculator does not always adjust  
        //        //resource calculator amounts, such as fuel and labor, when 
        //        //Input.Times change (if it did, the following would not be needed).
        //        if (CalculatorHelpers.IsLinkedViewXmlDoc(
        //            this.GCCalculatorParams.CurrentElementURIPattern,
        //            this.GCCalculatorParams.ExtensionDocToCalcURI))
        //        {
        //            //7. replace the this.GCCalculatorParams.CalculationsElement when 
        //            //the originating doctocalcuri node is processed
        //            bool bHasReplacedCalculator = CalculatorHelpers.ReplaceCalculations(
        //                this.GCCalculatorParams, currentElement,
        //                linkedViewElement);
        //            //8. this pattern combines analyzer and calculator patterns
        //            CalculatorHelpers.SetAnalyzerParameters(
        //               this.GCCalculatorParams, linkedViewElement);
        //        }
        //        //9. SetXmlDocAttributes
        //        CalculatorHelpers.SetXmlDocUpdates(this.GCCalculatorParams,
        //            linkedViewElement, ref currentElement,
        //            this.GCCalculatorParams.Updates);
        //    }
        //    else
        //    {
        //        //version 1.3.0 added this so that ancestors are always added to collections
        //        //to make analyses consistent (i.e. always start with group)
        //        GCArguments.CurrentElement = currentElement;
        //        //ancestors can never update or use a linkedview element
        //        GCArguments.LinkedViewElement = null;
        //        //but we want the ancestor current element added to the underlying collections
        //        await RunCalculatorAsync(GCArguments);
        //        //always return true, so no error msg is generated
        //        bHasCalculations = true;
        //    }
        //    return bHasCalculations;
        //}
        //private void AdjustSpecialtyLinkedViewElements(XElement currentElement,
        //    XElement linkedViewElement)
        //{
        //    if (this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.SubAppType
        //        == Constants.SUBAPPLICATION_TYPES.locals.ToString())
        //    {
        //        if (currentElement.Name.LocalName
        //            == Local.LOCAL_TYPES.localaccountgroup.ToString())
        //        {
        //            linkedViewElement = null;
        //        }
        //        else if (currentElement.Name.LocalName
        //            == Local.LOCAL_TYPES.local.ToString())
        //        {
        //            //local nodes are treated the same as children of starting group node
        //            this.GCCalculatorParams.NeedsCalculators = true;
        //            //locals have already been inserted
        //            this.GCCalculatorParams.NeedsXmlDocOnly = true;
        //            linkedViewElement = new XElement(this.GCCalculatorParams.LinkedViewElement);
        //        }
        //    }
        //}
        //private async Task<bool> RunStatisticalCalculationAsync(XElement currentElement)
        //{
        //    bool bHasCalculations = false;
        //    //1. set parameters needed by updates collection
        //    this.GCCalculatorParams.CurrentElementNodeName
        //        = currentElement.Name.LocalName;
        //    this.GCCalculatorParams.CurrentElementURIPattern
        //        = CalculatorHelpers.MakeNewURIPatternFromElement(
        //        this.GCCalculatorParams.ExtensionDocToCalcURI.URIPattern,
        //        currentElement);
        //    GCArguments.CurrentElement = currentElement;
        //    await RunCalculatorAsync(GCArguments);
        //    currentElement = GCArguments.CurrentElement;
        //    bHasCalculations = GCArguments.HasCalculations;
        //    if (!bHasCalculations
        //       && string.IsNullOrEmpty(this.GCCalculatorParams.ErrorMessage))
        //        this.GCCalculatorParams.ErrorMessage
        //            += Errors.MakeStandardErrorMsg("ANALYSES_CANTRUN");
        //    return bHasCalculations;
        //}
        //private bool NeedsRelatedCalculator()
        //{
        //    bool bNeedsRelatedCalculator = false;
        //    if ((this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.SubAppType
        //        != Constants.SUBAPPLICATION_TYPES.inputprices.ToString()
        //        && this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.SubAppType
        //        != Constants.SUBAPPLICATION_TYPES.outputprices.ToString())
        //        && (this.GCCalculatorParams.CurrentElementNodeName.EndsWith(
        //        Input.INPUT_PRICE_TYPES.input.ToString())
        //        || this.GCCalculatorParams.CurrentElementNodeName.EndsWith(
        //        Output.OUTPUT_PRICE_TYPES.output.ToString())))
        //    {
        //        bNeedsRelatedCalculator = true;
        //    }
        //    return bNeedsRelatedCalculator;
        //}
        //private void ChangeLinkedViewCalculator(XElement currentElement, XElement linkedViewElement)
        //{
        //    //v137 pattern allows analyzers to update descendents using dbupdates
        //    //i.e. need i/o calculators to get totals, but don't want to 
        //    //overwrite those calculations in db
        //    if (this.GCCalculatorParams.ExtensionCalcDocURI.URIDataManager.HostName
        //        == DevTreks.Data.Helpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString()
        //        && this.GCCalculatorParams.NeedsCalculators
        //        && CalculatorHelpers.IsSelfOrChildNode(this.GCCalculatorParams, currentElement.Name.LocalName))
        //    {
        //        //100% Rule 1: Analyzers never, ever, update calculators
        //        string sCalculatorType = CalculatorHelpers.GetAttribute(linkedViewElement,
        //            Calculator1.cCalculatorType);
        //        //pure calculators never have an analysis type
        //        string sAnalysisType = CalculatorHelpers.GetAttribute(linkedViewElement,
        //            Calculator1.cAnalyzerType);
        //        if (!string.IsNullOrEmpty(sCalculatorType)
        //            && string.IsNullOrEmpty(sAnalysisType))
        //        {
        //            //order of lv retrieval gets calulators before analyzers
        //            XElement analyzerLV = CalculatorHelpers.GetChildLinkedViewUsingAttribute(
        //                currentElement, Calculator1.cAnalyzerType, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
        //            if (analyzerLV != null)
        //            {
        //                if (this.GCCalculatorParams.LinkedViewElement != null)
        //                {
        //                    //keep the id and calculatorid, but update the rest of the atts with new lv
        //                    string sId = CalculatorHelpers.GetAttribute(analyzerLV, Calculator1.cId);
        //                    string sCalcId = CalculatorHelpers.GetAttribute(analyzerLV, Calculator1.cCalculatorId);
        //                    analyzerLV = new XElement(this.GCCalculatorParams.LinkedViewElement);
        //                    CalculatorHelpers.SetAttribute(analyzerLV, Calculator1.cId, sId);
        //                    CalculatorHelpers.SetAttribute(analyzerLV, Calculator1.cCalculatorId, sCalcId);
        //                }
        //                //use it to update db (not the calculator)
        //                linkedViewElement = new XElement(analyzerLV);
        //            }
        //            else
        //            {
        //                //use the base linked view standard pattern
        //                //avoids updating the wrong lvs
        //                linkedViewElement = CalculatorHelpers.GetNewCalculator(this.GCCalculatorParams, currentElement);
        //            }
        //        }
        //    }
        //    //100% Rule 2: Analyzers and Calculators never, ever, allow descendent lvs 
        //    //to have parent Overwrite or UseSameCalc properties
        //    if (this.GCCalculatorParams.StartingDocToCalcNodeName
        //        != currentElement.Name.LocalName)
        //    {
        //        if (linkedViewElement != null)
        //        {
        //            string sAttValue = CalculatorHelpers.GetAttribute(linkedViewElement,
        //                Calculator1.cUseSameCalculator);
        //            if ((!string.IsNullOrEmpty(sAttValue)) && sAttValue != Constants.NONE)
        //            {
        //                CalculatorHelpers.SetAttribute(linkedViewElement,
        //                    Calculator1.cUseSameCalculator, string.Empty);
        //            }
        //            sAttValue = CalculatorHelpers.GetAttribute(linkedViewElement,
        //                Calculator1.cOverwrite);
        //            if ((!string.IsNullOrEmpty(sAttValue)) && sAttValue != Constants.NONE)
        //            {
        //                CalculatorHelpers.SetAttribute(linkedViewElement,
        //                    Calculator1.cOverwrite, string.Empty);
        //            }
        //        }
        //    }
        //}
        //private void ChangeLinkedViewCalculatorForAnalysis(XElement currentElement, XElement linkedViewElement)
        //{
        //    //v137 pattern allows analyzers to update descendents using dbupdates
        //    //i.e. need i/o calculators to get totals, but don't want to 
        //    //overwrite those calculations in db
        //    if (this.GCCalculatorParams.ExtensionCalcDocURI.URIDataManager.HostName
        //        == DevTreks.Data.Helpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString()
        //        && this.GCCalculatorParams.NeedsCalculators
        //        && CalculatorHelpers.IsSelfOrChildNode(this.GCCalculatorParams, currentElement.Name.LocalName))
        //    {
        //        //100% Rule 1: Analyzers never, ever, update calculators
        //        string sCalculatorType = CalculatorHelpers.GetAttribute(linkedViewElement,
        //            Calculator1.cCalculatorType);
        //        //pure calculators never have an analysis type
        //        string sAnalysisType = CalculatorHelpers.GetAttribute(linkedViewElement,
        //            Calculator1.cAnalyzerType);
        //        //some analyzers include calculators (lca)
        //        if (!string.IsNullOrEmpty(sCalculatorType)
        //            && string.IsNullOrEmpty(sAnalysisType))
        //        {
        //            //order of lv retrieval gets calulators before analyzers
        //            XElement analyzerLV = CalculatorHelpers.GetChildLinkedViewUsingAttribute(
        //                currentElement, Calculator1.cAnalyzerType, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
        //            if (analyzerLV != null)
        //            {
        //                if (this.GCCalculatorParams.LinkedViewElement != null)
        //                {
        //                    //keep the id and calculatorid, but update the rest of the atts with new lv
        //                    string sId = CalculatorHelpers.GetAttribute(analyzerLV, Calculator1.cId);
        //                    string sCalcId = CalculatorHelpers.GetAttribute(analyzerLV, Calculator1.cCalculatorId);
        //                    analyzerLV = new XElement(this.GCCalculatorParams.LinkedViewElement);
        //                    CalculatorHelpers.SetAttribute(analyzerLV, Calculator1.cId, sId);
        //                    CalculatorHelpers.SetAttribute(analyzerLV, Calculator1.cCalculatorId, sCalcId);
        //                }
        //                //use it to update db (not the calculator)
        //                linkedViewElement = new XElement(analyzerLV);
        //            }
        //            else
        //            {
        //                //use the base linked view standard pattern
        //                //avoids updating the wrong lvs
        //                linkedViewElement = CalculatorHelpers.GetNewCalculator(this.GCCalculatorParams, currentElement);
        //            }
        //        }
        //        else if (string.IsNullOrEmpty(sCalculatorType)
        //            && !string.IsNullOrEmpty(sAnalysisType))
        //        {
        //            //analyzers can insert or update children analyzers
        //            XElement analyzerLV = CalculatorHelpers.GetChildLinkedViewUsingAttribute(
        //                currentElement, Calculator1.cAnalyzerType, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
        //            if (analyzerLV != null)
        //            {
        //                if (this.GCCalculatorParams.LinkedViewElement != null)
        //                {
        //                    //keep the id and calculatorid, but update the rest of the atts with new lv
        //                    string sId = CalculatorHelpers.GetAttribute(analyzerLV, Calculator1.cId);
        //                    string sCalcId = CalculatorHelpers.GetAttribute(analyzerLV, Calculator1.cCalculatorId);
        //                    analyzerLV = new XElement(this.GCCalculatorParams.LinkedViewElement);
        //                    CalculatorHelpers.SetAttribute(analyzerLV, Calculator1.cId, sId);
        //                    CalculatorHelpers.SetAttribute(analyzerLV, Calculator1.cCalculatorId, sCalcId);
        //                }
        //                //use it to update db (not the calculator)
        //                linkedViewElement = new XElement(analyzerLV);
        //            }
        //            else
        //            {
        //                //use the base linked view standard pattern
        //                //avoids updating the wrong lvs
        //                linkedViewElement = CalculatorHelpers.GetNewCalculator(this.GCCalculatorParams, currentElement);
        //            }
        //        }
        //    }
        //    //100% Rule 2: Analyzers and Calculators never, ever, allow descendent lvs 
        //    //to have parent Overwrite or UseSameCalc properties
        //    if (this.GCCalculatorParams.StartingDocToCalcNodeName
        //        != currentElement.Name.LocalName)
        //    {
        //        if (linkedViewElement != null)
        //        {
        //            string sAttValue = CalculatorHelpers.GetAttribute(linkedViewElement,
        //                Calculator1.cUseSameCalculator);
        //            if ((!string.IsNullOrEmpty(sAttValue)) && sAttValue != Constants.NONE)
        //            {
        //                CalculatorHelpers.SetAttribute(linkedViewElement,
        //                    Calculator1.cUseSameCalculator, string.Empty);
        //            }
        //            sAttValue = CalculatorHelpers.GetAttribute(linkedViewElement,
        //                Calculator1.cOverwrite);
        //            if ((!string.IsNullOrEmpty(sAttValue)) && sAttValue != Constants.NONE)
        //            {
        //                CalculatorHelpers.SetAttribute(linkedViewElement,
        //                    Calculator1.cOverwrite, string.Empty);
        //            }
        //        }
        //    }
        //}
        
        
        ////allows derived classes to override the default streaming 
        ////and save method
        //protected virtual async Task<bool> RunDevPackCalculationsAsync()
        //{
        //    bool bHasCalculations = false;
        //    IDictionary<string, string> fileOrFolderPaths =
        //        CalculatorHelpers.GetDevPackState(this.GCCalculatorParams);
        //    if (string.IsNullOrEmpty(this.GCCalculatorParams.ErrorMessage))
        //    {
        //        if (fileOrFolderPaths != null)
        //        {
        //            if (fileOrFolderPaths.Count > 0)
        //            {
        //                string sId = string.Empty;
        //                string sDocToCalcFilePath = string.Empty;
        //                string sTempDocToCalcPath = string.Empty;
        //                int i = 0;
        //                foreach (KeyValuePair<string, string> kvp
        //                    in fileOrFolderPaths)
        //                {
        //                    bHasCalculations = false;
        //                    sId = kvp.Key;
        //                    sDocToCalcFilePath = kvp.Value;
        //                    if (CalculatorHelpers.URIAbsoluteExists(sDocToCalcFilePath))
        //                    {
        //                        //make new calcparams using sDocToCalcFilePath
        //                        ExtensionContentURI devPackCalcURI
        //                            = CalculatorHelpers.GetDevPackCalcURI(this.GCCalculatorParams.ExtensionDocToCalcURI,
        //                            this.GCCalculatorParams.ExtensionCalcDocURI, sDocToCalcFilePath,
        //                            ref sTempDocToCalcPath);
        //                        //copy the doctocalc to tempdoctocalcpath
        //                        CalculatorHelpers.CopyFiles(sDocToCalcFilePath,
        //                            sTempDocToCalcPath);
        //                        //reset calculatorparams to reflect devPackCalcURI
        //                        CalculatorParameters devPackCalcParameters
        //                            = new CalculatorParameters(this.GCCalculatorParams);
        //                        devPackCalcParameters.CurrentElementNodeName
        //                            = devPackCalcURI.URINodeName;
        //                        devPackCalcParameters.CurrentElementURIPattern
        //                            = devPackCalcURI.URIPattern;
        //                        //reset the calcparams.extdoctocalcuri
        //                        devPackCalcParameters.ExtensionDocToCalcURI
        //                            = devPackCalcURI;

        //                        //currentDevPackElement has current db-generated linked views
        //                        //and is more reliable for updating linked views in db
        //                        XElement currentDevPackElement = null;
        //                        XElement linkedViewElement = null;
        //                        CalculatorHelpers.GetDevPackCalculator(this.GCCalculatorParams,
        //                            devPackCalcParameters, out currentDevPackElement,
        //                            out linkedViewElement);
        //                        //4. raise event to carry out calculations
        //                        GCArguments.CurrentElement = currentDevPackElement;
        //                        GCArguments.LinkedViewElement = linkedViewElement;
        //                        GCArguments.CalculatorParams = devPackCalcParameters;
        //                        GCArguments.CalculatorParams.CurrentElement = currentDevPackElement;
        //                        GCArguments.CalculatorParams.LinkedViewElement = linkedViewElement;
        //                        await RunDevPackCalculationsAsync(GCArguments);
        //                        bHasCalculations = GCArguments.HasCalculations;
        //                        currentDevPackElement = GCArguments.CurrentElement;
        //                        linkedViewElement = GCArguments.LinkedViewElement;
        //                        //5. SetXmlDocAttributes (using devPackElement)
        //                        if (bHasCalculations)
        //                        {
        //                            //updates for children
        //                            CalculatorHelpers.SetXmlDocUpdates(devPackCalcParameters,
        //                                linkedViewElement, ref currentDevPackElement,
        //                                this.GCCalculatorParams.Updates);
        //                            //add the tempdoctocalcpath to updates for potential saving later
        //                            string sErrorMsg = string.Empty;
        //                            CalculatorHelpers.SetDevPackUpdatesState(
        //                                devPackCalcURI, this.GCCalculatorParams.ExtensionCalcDocURI,
        //                                this.GCCalculatorParams.Updates, ref sErrorMsg);
        //                            if (i == 0)
        //                            {
        //                                //set the parent devpack
        //                                this.GCCalculatorParams.CurrentElementNodeName
        //                                    = this.GCCalculatorParams.ExtensionDocToCalcURI.URINodeName;
        //                                this.GCCalculatorParams.CurrentElementURIPattern
        //                                    = this.GCCalculatorParams.ExtensionDocToCalcURI.URIPattern;
        //                                //currentDevPackElement has current db-generated linked views
        //                                //and is reliable for updating linked views in db
        //                                CalculatorHelpers.GetDevPackCalculator(this.GCCalculatorParams,
        //                                    this.GCCalculatorParams, out currentDevPackElement,
        //                                    out linkedViewElement);
        //                                //update the parent
        //                                this.GCCalculatorParams.NeedsXmlDocOnly = true;
        //                                CalculatorHelpers.SetXmlDocUpdates(this.GCCalculatorParams,
        //                                    linkedViewElement, ref currentDevPackElement,
        //                                    this.GCCalculatorParams.Updates);
        //                                //show them the first calculated document as feedback
        //                                //refactor for future to show a summary of all calculated custom docs
        //                                //move the doctocalc to tempdoctocalcpath
        //                                CalculatorHelpers.CopyFiles(sTempDocToCalcPath,
        //                                    this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
        //                            }
        //                        }
        //                    }
        //                    i++;
        //                }
        //            }
        //        }
        //    }
        //    return bHasCalculations;
        //}
        
    }
}

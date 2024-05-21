using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using VerifyCS = Analyzer1.Test.CSharpCodeFixVerifier<
    Analyzer1.Analyzer1Analyzer,
    Analyzer1.Analyzer1CodeFixProvider>;

using VerifyF_CS = Analyzer1.Test.CSharpCodeFixVerifier<
    AnalyzerF.AnalyzerFAnalyzer,
    Analyzer1.Analyzer1CodeFixProvider>;

namespace Analyzer1.Test
{
    [TestClass]
    public class Analyzer1UnitTest
    {
        const string sampleTrigger1 = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class {|#0:TypeName|}
        {   
        }
    }";
        const string sampleFix1 = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
        }
    }";
        //No diagnostics expected to show up
        [TestMethod]
        public async Task TestMethodNull()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
            await VerifyF_CS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task TestMethodDiagAndFixC()
        {
            var test = sampleTrigger1;
            var fixtest = sampleFix1;

            var expected = VerifyCS.Diagnostic("Analyzer1").WithLocation(0).WithArguments("TypeName");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task TestMethodDiagAndFixF()
        {
            var test = sampleTrigger1;
            var fixtest = sampleFix1;

            //var expected = VerifyCS.Diagnostic("AnalyzerF").WithLocation(0).WithArguments("TypeName");
            var expected = VerifyCS.Diagnostic(AnalyzerF.AnalyzerHelpers.rule).WithLocation(0).WithArguments("TypeName");
            //var expected2 = VerifyCS.Diagnostic("Analyzer1").WithLocation(0).WithArguments("TypeName");
            //await VerifyCS.VerifyCodeFixAsync(test, new[] { expected, expected2 }, fixtest);
            await VerifyF_CS.VerifyCodeFixAsync(test, expected, fixtest);
        }
    }
}

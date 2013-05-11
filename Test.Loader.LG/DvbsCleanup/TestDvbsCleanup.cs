﻿using System.IO;
using ChanSort.Loader.LG;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Loader.LG
{
  [TestClass]
  public class TestDvbsCleanup : TestBase
  {
    [TestMethod]
    public void TestLM620S_WithSatChannels()
    {
      // "VitorMartinsAugusto"
      this.ExecuteTest("DvbsCleanup/xxLM620S-ZE00001");
    }

    [TestMethod]
    public void TestLM860V_WithoutSatChannels()
    {
      // "PDA-User"
      this.ExecuteTest("DvbsCleanup/xxLM860V-ZB99998");
    }

    [TestMethod]
    public void TestLM640T_WithBogusDvbsBlock()
    {
      // "OmarGadzhiev"
      this.ExecuteTest("DvbsCleanup/xxLM640T-ZA00000");
    }


    private void ExecuteTest(string modelAndBaseName, bool generateReferenceFile = false)
    {
      // copy required input and assertion files
      DeploymentItem("ChanSort.Loader.LG\\ChanSort.Loader.LG.ini");
      DeploymentItem("Test.Loader.LG\\" + modelAndBaseName + ".TLL.in");
      DeploymentItem("Test.Loader.LG\\" + modelAndBaseName + ".TLL.out");

      var baseName = Path.GetFileNameWithoutExtension(modelAndBaseName);

      // load the TLL file
      TllFileSerializerPlugin plugin = new TllFileSerializerPlugin();
      var serializer = (TllFileSerializer)plugin.CreateSerializer(baseName + ".TLL.in");
      serializer.IsTesting = true;
      serializer.Load();
      serializer.DataRoot.ApplyCurrentProgramNumbers();     

      // save TLL file and compare to reference file
      serializer.CleanUpChannelData();
      serializer.Save(tempFile);
      if (generateReferenceFile)
        File.Copy(tempFile, this.GetSolutionBaseDir() + "\\Test.Loader.LG\\" + modelAndBaseName + ".TLL.out", true);
      else
        AssertBinaryFileContent(tempFile, baseName + ".TLL.out");      
    }
  }
}

<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, January -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Investment"
	xmlns:DisplayDevPacks="urn:displaydevpacks">
	<xsl:output method="xml" indent="yes" omit-xml-declaration="yes" encoding="UTF-8" />
	<!-- pass in params -->
	<!-- what action is being taken by the server -->
	<xsl:param name="serverActionType" />
	<!-- what other action is being taken by the server -->
	<xsl:param name="serverSubActionType" />
	<!-- is the member viewing this uri the owner? -->
	<xsl:param name="isURIOwningClub" />
	<!-- which node to start with? -->
	<xsl:param name="nodeName" />
	<!-- which view to use? -->
	<xsl:param name="viewEditType" />
	<!-- is this a coordinator? -->
	<xsl:param name="memberRole" />
	<!-- what is the current uri? -->
	<xsl:param name="selectedFileURIPattern" />
	<!-- the addin being used -->
	<xsl:param name="calcDocURI" />
	<!-- the node being calculated-->
	<xsl:param name="docToCalcNodeName" />
	<!-- standard params used with calcs and custom docs -->
	<xsl:param name="calcParams" />
	<!-- what is the name of the node to be selected? -->
	<xsl:param name="selectionsNodeNeededName" />
	<!-- which network is this doc from? -->
	<xsl:param name="networkId" />
	<!-- what is the start row? -->
	<xsl:param name="startRow" />
	<!-- what is the end row? -->
	<xsl:param name="endRow" />
	<!-- what are the pagination properties ? -->
	<xsl:param name="pageParams" />
	<!-- what is the guide's email? -->
	<xsl:param name="clubEmail" />
	<!-- what is the current serviceid? -->
	<xsl:param name="contenturipattern" />
	<!-- path to resources -->
	<xsl:param name="fullFilePath" />
	<!-- init html -->
	<xsl:template match="@*|/|node()" />
	<xsl:template match="/">
		<xsl:apply-templates select="root" />
	</xsl:template>
	<xsl:template match="root">
		<div id="modEdits_divEditsDoc">
      <xsl:apply-templates select="servicebase" />
			<xsl:apply-templates select="investmentgroup" />
			<div>
				<a id="aFeedback" name="Feedback">
					<xsl:attribute name="href">mailto:<xsl:value-of select="$clubEmail" />?subject=<xsl:value-of select="$selectedFileURIPattern" /></xsl:attribute>
					Feedback About <xsl:value-of select="$selectedFileURIPattern" />
				</a>
      </div>
		</div>
	</xsl:template>
	<xsl:template match="servicebase">
		<h4 class="ui-bar-b">
			Service: <xsl:value-of select="@Name" />
		</h4>
		<xsl:apply-templates select="investmentgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentgroup">
    <h4>
      <strong>Investment Group</strong>&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mestat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investment">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investment">
		<h4>
      <strong>Investment</strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mestat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investmenttimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmenttimeperiod">
		<h4>
      <strong>Time Period</strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
    <xsl:apply-templates select="root/linkedview[@AnalyzerType='mestat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="investmentoutcomes" />
		<xsl:apply-templates select="investmentcomponents" />
	</xsl:template>
	<xsl:template match="investmentoutcomes">
		<xsl:apply-templates select="investmentoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutcome">
		<h4>
      <strong>Outcome </strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mestat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	<xsl:apply-templates select="investmentoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutput">
    <h4>
      <strong>Output </strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mestat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentcomponents">
		<xsl:apply-templates select="investmentcomponent">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentcomponent">
		<h4>
      <strong>Component</strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mestat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	<xsl:apply-templates select="investmentinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentinput">
		<h4>
      <strong>Input</strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mestat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:if test="(@TME2Name1 != '')">
		<div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicator Totals</strong>
      </h4>
      <xsl:if test="(@TotalME2Type1 != '' and @TotalME2Type1 != 'none')">
        <div>
			    M and E Type: <strong><xsl:value-of select="@TotalME2Type1"/></strong>
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name1 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 1 Name : <strong><xsl:value-of select="@TME2Name1"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label1"/>
          </div>
          <div class="ui-block-a">
            Observations : <xsl:value-of select="@TME2N1"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total1"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit1"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2Mean1"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2Median1"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2Variance1"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2StandDev1"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total1"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit1"/>    
          </div>
          <div class="ui-block-a">
            Q1 Mean : <xsl:value-of select="@TME2Q1Mean1"/>
          </div>
          <div class="ui-block-b">
            Q1 Median : <xsl:value-of select="@TME2Q1Median1"/>
          </div>
          <div class="ui-block-a">
            Q1 Variance : <xsl:value-of select="@TME2Q1Variance1"/>
          </div>
          <div class="ui-block-b">    
            Q1 Std Dev : <xsl:value-of select="@TME2Q1StandDev1"/>
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total1"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit1"/>    
          </div>
          <div class="ui-block-a">
            Q2 Mean : <xsl:value-of select="@TME2Q2Mean1"/>
          </div>
          <div class="ui-block-b">
            Q2 Median : <xsl:value-of select="@TME2Q2Median1"/>
          </div>
          <div class="ui-block-a">
            Q2 Variance : <xsl:value-of select="@TME2Q2Variance1"/>
          </div>
          <div class="ui-block-b">    
            Q2 Std Dev : <xsl:value-of select="@TME2Q2StandDev1"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description1" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name2 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 1 Name : <strong><xsl:value-of select="@TME2Name2"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label2"/>
          </div>
          <div class="ui-block-a">
            Observations : <xsl:value-of select="@TME2N2"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total2"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit2"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2Mean2"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2Median2"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2Variance2"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2StandDev2"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total2"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit2"/>    
          </div>
          <div class="ui-block-a">
            Q1 Mean : <xsl:value-of select="@TME2Q1Mean2"/>
          </div>
          <div class="ui-block-b">
            Q1 Median : <xsl:value-of select="@TME2Q1Median2"/>
          </div>
          <div class="ui-block-a">
            Q1 Variance : <xsl:value-of select="@TME2Q1Variance2"/>
          </div>
          <div class="ui-block-b">    
            Q1 Std Dev : <xsl:value-of select="@TME2Q1StandDev2"/>
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total2"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit2"/>    
          </div>
          <div class="ui-block-a">
            Q2 Mean : <xsl:value-of select="@TME2Q2Mean2"/>
          </div>
          <div class="ui-block-b">
            Q2 Median : <xsl:value-of select="@TME2Q2Median2"/>
          </div>
          <div class="ui-block-a">
            Q2 Variance : <xsl:value-of select="@TME2Q2Variance2"/>
          </div>
          <div class="ui-block-b">    
            Q2 Std Dev : <xsl:value-of select="@TME2Q2StandDev2"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description2" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name3 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 1 Name : <strong><xsl:value-of select="@TME2Name3"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label3"/>
          </div>
          <div class="ui-block-a">
            Observations : <xsl:value-of select="@TME2N3"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total3"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit3"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2Mean3"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2Median3"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2Variance3"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2StandDev3"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total3"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit3"/>    
          </div>
          <div class="ui-block-a">
            Q1 Mean : <xsl:value-of select="@TME2Q1Mean3"/>
          </div>
          <div class="ui-block-b">
            Q1 Median : <xsl:value-of select="@TME2Q1Median3"/>
          </div>
          <div class="ui-block-a">
            Q1 Variance : <xsl:value-of select="@TME2Q1Variance3"/>
          </div>
          <div class="ui-block-b">    
            Q1 Std Dev : <xsl:value-of select="@TME2Q1StandDev3"/>
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total3"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit3"/>    
          </div>
          <div class="ui-block-a">
            Q2 Mean : <xsl:value-of select="@TME2Q2Mean3"/>
          </div>
          <div class="ui-block-b">
            Q2 Median : <xsl:value-of select="@TME2Q2Median3"/>
          </div>
          <div class="ui-block-a">
            Q2 Variance : <xsl:value-of select="@TME2Q2Variance3"/>
          </div>
          <div class="ui-block-b">    
            Q2 Std Dev : <xsl:value-of select="@TME2Q2StandDev3"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description3" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name4 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 1 Name : <strong><xsl:value-of select="@TME2Name4"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label4"/>
          </div>
          <div class="ui-block-a">
            Observations : <xsl:value-of select="@TME2N4"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total4"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit4"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2Mean4"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2Median4"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2Variance4"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2StandDev4"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total4"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit4"/>    
          </div>
          <div class="ui-block-a">
            Q1 Mean : <xsl:value-of select="@TME2Q1Mean4"/>
          </div>
          <div class="ui-block-b">
            Q1 Median : <xsl:value-of select="@TME2Q1Median4"/>
          </div>
          <div class="ui-block-a">
            Q1 Variance : <xsl:value-of select="@TME2Q1Variance4"/>
          </div>
          <div class="ui-block-b">    
            Q1 Std Dev : <xsl:value-of select="@TME2Q1StandDev4"/>
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total4"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit4"/>    
          </div>
          <div class="ui-block-a">
            Q2 Mean : <xsl:value-of select="@TME2Q2Mean4"/>
          </div>
          <div class="ui-block-b">
            Q2 Median : <xsl:value-of select="@TME2Q2Median4"/>
          </div>
          <div class="ui-block-a">
            Q2 Variance : <xsl:value-of select="@TME2Q2Variance4"/>
          </div>
          <div class="ui-block-b">    
            Q2 Std Dev : <xsl:value-of select="@TME2Q2StandDev4"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description4" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name5 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 1 Name : <strong><xsl:value-of select="@TME2Name5"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label5"/>
          </div>
          <div class="ui-block-a">
            Observations : <xsl:value-of select="@TME2N5"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total5"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit5"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2Mean5"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2Median5"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2Variance5"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2StandDev5"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total5"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit5"/>    
          </div>
          <div class="ui-block-a">
            Q1 Mean : <xsl:value-of select="@TME2Q1Mean5"/>
          </div>
          <div class="ui-block-b">
            Q1 Median : <xsl:value-of select="@TME2Q1Median5"/>
          </div>
          <div class="ui-block-a">
            Q1 Variance : <xsl:value-of select="@TME2Q1Variance5"/>
          </div>
          <div class="ui-block-b">    
            Q1 Std Dev : <xsl:value-of select="@TME2Q1StandDev5"/>
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total5"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit5"/>    
          </div>
          <div class="ui-block-a">
            Q2 Mean : <xsl:value-of select="@TME2Q2Mean5"/>
          </div>
          <div class="ui-block-b">
            Q2 Median : <xsl:value-of select="@TME2Q2Median5"/>
          </div>
          <div class="ui-block-a">
            Q2 Variance : <xsl:value-of select="@TME2Q2Variance5"/>
          </div>
          <div class="ui-block-b">    
            Q2 Std Dev : <xsl:value-of select="@TME2Q2StandDev5"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description5" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name6 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 1 Name : <strong><xsl:value-of select="@TME2Name6"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label6"/>
          </div>
          <div class="ui-block-a">
            Observations : <xsl:value-of select="@TME2N6"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total6"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit6"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2Mean6"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2Median6"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2Variance6"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2StandDev6"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total6"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit6"/>    
          </div>
          <div class="ui-block-a">
            Q1 Mean : <xsl:value-of select="@TME2Q1Mean6"/>
          </div>
          <div class="ui-block-b">
            Q1 Median : <xsl:value-of select="@TME2Q1Median6"/>
          </div>
          <div class="ui-block-a">
            Q1 Variance : <xsl:value-of select="@TME2Q1Variance6"/>
          </div>
          <div class="ui-block-b">    
            Q1 Std Dev : <xsl:value-of select="@TME2Q1StandDev6"/>
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total6"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit6"/>    
          </div>
          <div class="ui-block-a">
            Q2 Mean : <xsl:value-of select="@TME2Q2Mean6"/>
          </div>
          <div class="ui-block-b">
            Q2 Median : <xsl:value-of select="@TME2Q2Median6"/>
          </div>
          <div class="ui-block-a">
            Q2 Variance : <xsl:value-of select="@TME2Q2Variance6"/>
          </div>
          <div class="ui-block-b">    
            Q2 Std Dev : <xsl:value-of select="@TME2Q2StandDev6"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description6" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name7 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 1 Name : <strong><xsl:value-of select="@TME2Name7"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label7"/>
          </div>
          <div class="ui-block-a">
            Observations : <xsl:value-of select="@TME2N7"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total7"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit7"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2Mean7"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2Median7"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2Variance7"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2StandDev7"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total7"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit7"/>    
          </div>
          <div class="ui-block-a">
            Q1 Mean : <xsl:value-of select="@TME2Q1Mean7"/>
          </div>
          <div class="ui-block-b">
            Q1 Median : <xsl:value-of select="@TME2Q1Median7"/>
          </div>
          <div class="ui-block-a">
            Q1 Variance : <xsl:value-of select="@TME2Q1Variance7"/>
          </div>
          <div class="ui-block-b">    
            Q1 Std Dev : <xsl:value-of select="@TME2Q1StandDev7"/>
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total7"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit7"/>    
          </div>
          <div class="ui-block-a">
            Q2 Mean : <xsl:value-of select="@TME2Q2Mean7"/>
          </div>
          <div class="ui-block-b">
            Q2 Median : <xsl:value-of select="@TME2Q2Median7"/>
          </div>
          <div class="ui-block-a">
            Q2 Variance : <xsl:value-of select="@TME2Q2Variance7"/>
          </div>
          <div class="ui-block-b">    
            Q2 Std Dev : <xsl:value-of select="@TME2Q2StandDev7"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description7" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name8 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 1 Name : <strong><xsl:value-of select="@TME2Name8"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label8"/>
          </div>
          <div class="ui-block-a">
            Observations : <xsl:value-of select="@TME2N8"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total8"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit8"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2Mean8"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2Median8"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2Variance8"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2StandDev8"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total8"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit8"/>    
          </div>
          <div class="ui-block-a">
            Q1 Mean : <xsl:value-of select="@TME2Q1Mean8"/>
          </div>
          <div class="ui-block-b">
            Q1 Median : <xsl:value-of select="@TME2Q1Median8"/>
          </div>
          <div class="ui-block-a">
            Q1 Variance : <xsl:value-of select="@TME2Q1Variance8"/>
          </div>
          <div class="ui-block-b">    
            Q1 Std Dev : <xsl:value-of select="@TME2Q1StandDev8"/>
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total8"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit8"/>    
          </div>
          <div class="ui-block-a">
            Q2 Mean : <xsl:value-of select="@TME2Q2Mean8"/>
          </div>
          <div class="ui-block-b">
            Q2 Median : <xsl:value-of select="@TME2Q2Median8"/>
          </div>
          <div class="ui-block-a">
            Q2 Variance : <xsl:value-of select="@TME2Q2Variance8"/>
          </div>
          <div class="ui-block-b">    
            Q2 Std Dev : <xsl:value-of select="@TME2Q2StandDev8"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description8" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name9 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 1 Name : <strong><xsl:value-of select="@TME2Name9"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label9"/>
          </div>
          <div class="ui-block-a">
            Observations : <xsl:value-of select="@TME2N9"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total9"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit9"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2Mean9"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2Median9"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2Variance9"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2StandDev9"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total9"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit9"/>    
          </div>
          <div class="ui-block-a">
            Q1 Mean : <xsl:value-of select="@TME2Q1Mean9"/>
          </div>
          <div class="ui-block-b">
            Q1 Median : <xsl:value-of select="@TME2Q1Median9"/>
          </div>
          <div class="ui-block-a">
            Q1 Variance : <xsl:value-of select="@TME2Q1Variance9"/>
          </div>
          <div class="ui-block-b">    
            Q1 Std Dev : <xsl:value-of select="@TME2Q1StandDev9"/>
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total9"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit9"/>    
          </div>
          <div class="ui-block-a">
            Q2 Mean : <xsl:value-of select="@TME2Q2Mean9"/>
          </div>
          <div class="ui-block-b">
            Q2 Median : <xsl:value-of select="@TME2Q2Median9"/>
          </div>
          <div class="ui-block-a">
            Q2 Variance : <xsl:value-of select="@TME2Q2Variance9"/>
          </div>
          <div class="ui-block-b">    
            Q2 Std Dev : <xsl:value-of select="@TME2Q2StandDev9"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description9" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name10 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 1 Name : <strong><xsl:value-of select="@TME2Name10"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label10"/>
          </div>
          <div class="ui-block-a">
            Observations : <xsl:value-of select="@TME2N10"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total10"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit10"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2Mean10"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2Median10"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2Variance10"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2StandDev10"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total10"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit10"/>    
          </div>
          <div class="ui-block-a">
            Q1 Mean : <xsl:value-of select="@TME2Q1Mean10"/>
          </div>
          <div class="ui-block-b">
            Q1 Median : <xsl:value-of select="@TME2Q1Median10"/>
          </div>
          <div class="ui-block-a">
            Q1 Variance : <xsl:value-of select="@TME2Q1Variance10"/>
          </div>
          <div class="ui-block-b">    
            Q1 Std Dev : <xsl:value-of select="@TME2Q1StandDev10"/>
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total10"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit10"/>    
          </div>
          <div class="ui-block-a">
            Q2 Mean : <xsl:value-of select="@TME2Q2Mean10"/>
          </div>
          <div class="ui-block-b">
            Q2 Median : <xsl:value-of select="@TME2Q2Median10"/>
          </div>
          <div class="ui-block-a">
            Q2 Variance : <xsl:value-of select="@TME2Q2Variance10"/>
          </div>
          <div class="ui-block-b">    
            Q2 Std Dev : <xsl:value-of select="@TME2Q2StandDev10"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description10" />
	      </div>
      </xsl:if>
    </div>
    </xsl:if>
	</xsl:template>
</xsl:stylesheet>

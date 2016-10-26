<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2016, October -->
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
    <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicator Totals</strong>
      </h4>
      <xsl:if test="(@TME2Stage != '' and @TME2Stage != 'none')">
        <div>
          M and E Stage: <strong><xsl:value-of select="@TME2Stage"/></strong>
        </div>
      </xsl:if>
    <xsl:if test="(@TME2Name0 != '' and @TME2Name0 != 'none')">
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 1 Name : <strong><xsl:value-of select="@TME2Name0"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label0"/>
        </div>
        <div class="ui-block-a">
          Observations : <xsl:value-of select="@TME2N0"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Most Likely : <xsl:value-of select="@TME2TMAmount0"/>
        </div>
        <div class="ui-block-b">
          Unit : <xsl:value-of select="@TME2TMUnit0"/>
        </div>
        <div class="ui-block-a">
          Mean : <xsl:value-of select="@TME2MMean0"/>
        </div>
        <div class="ui-block-b">
          Median : <xsl:value-of select="@TME2MMedian0"/>
        </div>
        <div class="ui-block-a">
          Variance : <xsl:value-of select="@TME2MVariance0"/>
        </div>
        <div class="ui-block-b">    
          Std Dev : <xsl:value-of select="@TME2MStandDev0"/>
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount0"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TLUnit0"/>    
        </div>
        <div class="ui-block-a">
          Lower Mean : <xsl:value-of select="@TME2LMean0"/>
        </div>
        <div class="ui-block-b">
          Lower Median : <xsl:value-of select="@TME2LMedian0"/>
        </div>
        <div class="ui-block-a">
          Lower Variance : <xsl:value-of select="@TME2LVariance0"/>
        </div>
        <div class="ui-block-b">    
          Lower Std Dev : <xsl:value-of select="@TME2LStandDev0"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount0"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit0"/>    
        </div>
        <div class="ui-block-a">
          Upper Mean : <xsl:value-of select="@TME2UMean0"/>
        </div>
        <div class="ui-block-b">
          Upper Median : <xsl:value-of select="@TME2UMedian0"/>
        </div>
        <div class="ui-block-a">
          Upper Variance : <xsl:value-of select="@TME2UVariance0"/>
        </div>
        <div class="ui-block-b">    
          Upper Std Dev : <xsl:value-of select="@TME2UStandDev0"/>
        </div>
      </div>
      <div >
			  <strong>Description : </strong><xsl:value-of select="@TME2Description0" />
	    </div>
    </xsl:if>
    <xsl:if test="(@TME2Name1 != '' and @TME2Name1 != 'none')">
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
            Most Likely : <xsl:value-of select="@TME2TMAmount1"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2TMUnit1"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2MMean1"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2MMedian1"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2MVariance1"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2MStandDev1"/>
          </div>
          <div class="ui-block-a">
            Lower Total : <xsl:value-of select="@TME2TLAmount1"/>
          </div>
          <div class="ui-block-b">
            Lower Unit : <xsl:value-of select="@TME2TLUnit1"/>    
          </div>
          <div class="ui-block-a">
            Lower Mean : <xsl:value-of select="@TME2LMean1"/>
          </div>
          <div class="ui-block-b">
            Lower Median : <xsl:value-of select="@TME2LMedian1"/>
          </div>
          <div class="ui-block-a">
            Lower Variance : <xsl:value-of select="@TME2LVariance1"/>
          </div>
          <div class="ui-block-b">    
            Lower Std Dev : <xsl:value-of select="@TME2LStandDev1"/>
          </div>
          <div class="ui-block-a">
            Upper Most Likely : <xsl:value-of select="@TME2TUAmount1"/>
          </div>
          <div class="ui-block-b">
            Upper Unit : <xsl:value-of select="@TME2TUUnit1"/>    
          </div>
          <div class="ui-block-a">
            Upper Mean : <xsl:value-of select="@TME2UMean1"/>
          </div>
          <div class="ui-block-b">
            Upper Median : <xsl:value-of select="@TME2UMedian1"/>
          </div>
          <div class="ui-block-a">
            Upper Variance : <xsl:value-of select="@TME2UVariance1"/>
          </div>
          <div class="ui-block-b">    
            Upper Std Dev : <xsl:value-of select="@TME2UStandDev1"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description1" />
	      </div>
      </xsl:if>
    <xsl:if test="(@TME2Name2 != '' and @TME2Name2 != 'none')">
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
            Most Likely : <xsl:value-of select="@TME2TMAmount2"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2TMUnit2"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2MMean2"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2MMedian2"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2MVariance2"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2MStandDev2"/>
          </div>
          <div class="ui-block-a">
            Lower Total : <xsl:value-of select="@TME2TLAmount2"/>
          </div>
          <div class="ui-block-b">
            Lower Unit : <xsl:value-of select="@TME2TLUnit2"/>    
          </div>
          <div class="ui-block-a">
            Lower Mean : <xsl:value-of select="@TME2LMean2"/>
          </div>
          <div class="ui-block-b">
            Lower Median : <xsl:value-of select="@TME2LMedian2"/>
          </div>
          <div class="ui-block-a">
            Lower Variance : <xsl:value-of select="@TME2LVariance2"/>
          </div>
          <div class="ui-block-b">    
            Lower Std Dev : <xsl:value-of select="@TME2LStandDev2"/>
          </div>
          <div class="ui-block-a">
            Upper Total : <xsl:value-of select="@TME2TUAmount2"/>
          </div>
          <div class="ui-block-b">
            Upper Unit : <xsl:value-of select="@TME2TUUnit2"/>    
          </div>
          <div class="ui-block-a">
            Upper Mean : <xsl:value-of select="@TME2UMean2"/>
          </div>
          <div class="ui-block-b">
            Upper Median : <xsl:value-of select="@TME2UMedian2"/>
          </div>
          <div class="ui-block-a">
            Upper Variance : <xsl:value-of select="@TME2UVariance2"/>
          </div>
          <div class="ui-block-b">    
            Upper Std Dev : <xsl:value-of select="@TME2UStandDev2"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description2" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name3 != '' and @TME2Name3 != 'none')">
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
            Most Likely : <xsl:value-of select="@TME2TMAmount3"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2TMUnit3"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2MMean3"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2MMedian3"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2MVariance3"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2MStandDev3"/>
          </div>
          <div class="ui-block-a">
            Lower Total : <xsl:value-of select="@TME2TLAmount3"/>
          </div>
          <div class="ui-block-b">
            Lower Unit : <xsl:value-of select="@TME2TLUnit3"/>    
          </div>
          <div class="ui-block-a">
            Lower Mean : <xsl:value-of select="@TME2LMean3"/>
          </div>
          <div class="ui-block-b">
            Lower Median : <xsl:value-of select="@TME2LMedian3"/>
          </div>
          <div class="ui-block-a">
            Lower Variance : <xsl:value-of select="@TME2LVariance3"/>
          </div>
          <div class="ui-block-b">    
            Lower Std Dev : <xsl:value-of select="@TME2LStandDev3"/>
          </div>
          <div class="ui-block-a">
            Upper Total : <xsl:value-of select="@TME2TUAmount3"/>
          </div>
          <div class="ui-block-b">
            Upper Unit : <xsl:value-of select="@TME2TUUnit3"/>    
          </div>
          <div class="ui-block-a">
            Upper Mean : <xsl:value-of select="@TME2UMean3"/>
          </div>
          <div class="ui-block-b">
            Upper Median : <xsl:value-of select="@TME2UMedian3"/>
          </div>
          <div class="ui-block-a">
            Upper Variance : <xsl:value-of select="@TME2UVariance3"/>
          </div>
          <div class="ui-block-b">    
            Upper Std Dev : <xsl:value-of select="@TME2UStandDev3"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description3" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name4 != '' and @TME2Name4 != 'none')">
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
            Most Likely : <xsl:value-of select="@TME2TMAmount4"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2TMUnit4"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2MMean4"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2MMedian4"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2MVariance4"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2MStandDev4"/>
          </div>
          <div class="ui-block-a">
            Lower Total : <xsl:value-of select="@TME2TLAmount4"/>
          </div>
          <div class="ui-block-b">
            Lower Unit : <xsl:value-of select="@TME2TLUnit4"/>    
          </div>
          <div class="ui-block-a">
            Lower Mean : <xsl:value-of select="@TME2LMean4"/>
          </div>
          <div class="ui-block-b">
            Lower Median : <xsl:value-of select="@TME2LMedian4"/>
          </div>
          <div class="ui-block-a">
            Lower Variance : <xsl:value-of select="@TME2LVariance4"/>
          </div>
          <div class="ui-block-b">    
            Lower Std Dev : <xsl:value-of select="@TME2LStandDev4"/>
          </div>
          <div class="ui-block-a">
            Upper Total : <xsl:value-of select="@TME2TUAmount4"/>
          </div>
          <div class="ui-block-b">
            Upper Unit : <xsl:value-of select="@TME2TUUnit4"/>    
          </div>
          <div class="ui-block-a">
            Upper Mean : <xsl:value-of select="@TME2UMean4"/>
          </div>
          <div class="ui-block-b">
            Upper Median : <xsl:value-of select="@TME2UMedian4"/>
          </div>
          <div class="ui-block-a">
            Upper Variance : <xsl:value-of select="@TME2UVariance4"/>
          </div>
          <div class="ui-block-b">    
            Upper Std Dev : <xsl:value-of select="@TME2UStandDev4"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description4" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name5 != '' and @TME2Name5 != 'none')">
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
            Most Likely : <xsl:value-of select="@TME2TMAmount5"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2TMUnit5"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2MMean5"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2MMedian5"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2MVariance5"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2MStandDev5"/>
          </div>
          <div class="ui-block-a">
            Lower Total : <xsl:value-of select="@TME2TLAmount5"/>
          </div>
          <div class="ui-block-b">
            Lower Unit : <xsl:value-of select="@TME2TLUnit5"/>    
          </div>
          <div class="ui-block-a">
            Lower Mean : <xsl:value-of select="@TME2LMean5"/>
          </div>
          <div class="ui-block-b">
            Lower Median : <xsl:value-of select="@TME2LMedian5"/>
          </div>
          <div class="ui-block-a">
            Lower Variance : <xsl:value-of select="@TME2LVariance5"/>
          </div>
          <div class="ui-block-b">    
            Lower Std Dev : <xsl:value-of select="@TME2LStandDev5"/>
          </div>
          <div class="ui-block-a">
            Upper Total : <xsl:value-of select="@TME2TUAmount5"/>
          </div>
          <div class="ui-block-b">
            Upper Unit : <xsl:value-of select="@TME2TUUnit5"/>    
          </div>
          <div class="ui-block-a">
            Upper Mean : <xsl:value-of select="@TME2UMean5"/>
          </div>
          <div class="ui-block-b">
            Upper Median : <xsl:value-of select="@TME2UMedian5"/>
          </div>
          <div class="ui-block-a">
            Upper Variance : <xsl:value-of select="@TME2UVariance5"/>
          </div>
          <div class="ui-block-b">    
            Upper Std Dev : <xsl:value-of select="@TME2UStandDev5"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description5" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name6 != '' and @TME2Name6 != 'none')">
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
            Most Likely : <xsl:value-of select="@TME2TMAmount6"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2TMUnit6"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2MMean6"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2MMedian6"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2MVariance6"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2MStandDev6"/>
          </div>
          <div class="ui-block-a">
            Lower Total : <xsl:value-of select="@TME2TLAmount6"/>
          </div>
          <div class="ui-block-b">
            Lower Unit : <xsl:value-of select="@TME2TLUnit6"/>    
          </div>
          <div class="ui-block-a">
            Lower Mean : <xsl:value-of select="@TME2LMean6"/>
          </div>
          <div class="ui-block-b">
            Lower Median : <xsl:value-of select="@TME2LMedian6"/>
          </div>
          <div class="ui-block-a">
            Lower Variance : <xsl:value-of select="@TME2LVariance6"/>
          </div>
          <div class="ui-block-b">    
            Lower Std Dev : <xsl:value-of select="@TME2LStandDev6"/>
          </div>
          <div class="ui-block-a">
            Upper Total : <xsl:value-of select="@TME2TUAmount6"/>
          </div>
          <div class="ui-block-b">
            Upper Unit : <xsl:value-of select="@TME2TUUnit6"/>    
          </div>
          <div class="ui-block-a">
            Upper Mean : <xsl:value-of select="@TME2UMean6"/>
          </div>
          <div class="ui-block-b">
            Upper Median : <xsl:value-of select="@TME2UMedian6"/>
          </div>
          <div class="ui-block-a">
            Upper Variance : <xsl:value-of select="@TME2UVariance6"/>
          </div>
          <div class="ui-block-b">    
            Upper Std Dev : <xsl:value-of select="@TME2UStandDev6"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description6" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name7 != '' and @TME2Name7 != 'none')">
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
            Most Likely : <xsl:value-of select="@TME2TMAmount7"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2TMUnit7"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2MMean7"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2MMedian7"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2MVariance7"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2MStandDev7"/>
          </div>
          <div class="ui-block-a">
            Lower Total : <xsl:value-of select="@TME2TLAmount7"/>
          </div>
          <div class="ui-block-b">
            Lower Unit : <xsl:value-of select="@TME2TLUnit7"/>    
          </div>
          <div class="ui-block-a">
            Lower Mean : <xsl:value-of select="@TME2LMean7"/>
          </div>
          <div class="ui-block-b">
            Lower Median : <xsl:value-of select="@TME2LMedian7"/>
          </div>
          <div class="ui-block-a">
            Lower Variance : <xsl:value-of select="@TME2LVariance7"/>
          </div>
          <div class="ui-block-b">    
            Lower Std Dev : <xsl:value-of select="@TME2LStandDev7"/>
          </div>
          <div class="ui-block-a">
            Upper Total : <xsl:value-of select="@TME2TUAmount7"/>
          </div>
          <div class="ui-block-b">
            Upper Unit : <xsl:value-of select="@TME2TUUnit7"/>    
          </div>
          <div class="ui-block-a">
            Upper Mean : <xsl:value-of select="@TME2UMean7"/>
          </div>
          <div class="ui-block-b">
            Upper Median : <xsl:value-of select="@TME2UMedian7"/>
          </div>
          <div class="ui-block-a">
            Upper Variance : <xsl:value-of select="@TME2UVariance7"/>
          </div>
          <div class="ui-block-b">    
            Upper Std Dev : <xsl:value-of select="@TME2UStandDev7"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description7" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name8 != '' and @TME2Name8 != 'none')">
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
            Most Likely : <xsl:value-of select="@TME2TMAmount8"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2TMUnit8"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2MMean8"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2MMedian8"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2MVariance8"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2MStandDev8"/>
          </div>
          <div class="ui-block-a">
            Lower Total : <xsl:value-of select="@TME2TLAmount8"/>
          </div>
          <div class="ui-block-b">
            Lower Unit : <xsl:value-of select="@TME2TLUnit8"/>    
          </div>
          <div class="ui-block-a">
            Lower Mean : <xsl:value-of select="@TME2LMean8"/>
          </div>
          <div class="ui-block-b">
            Lower Median : <xsl:value-of select="@TME2LMedian8"/>
          </div>
          <div class="ui-block-a">
            Lower Variance : <xsl:value-of select="@TME2LVariance8"/>
          </div>
          <div class="ui-block-b">    
            Lower Std Dev : <xsl:value-of select="@TME2LStandDev8"/>
          </div>
          <div class="ui-block-a">
            Upper Total : <xsl:value-of select="@TME2TUAmount8"/>
          </div>
          <div class="ui-block-b">
            Upper Unit : <xsl:value-of select="@TME2TUUnit8"/>    
          </div>
          <div class="ui-block-a">
            Upper Mean : <xsl:value-of select="@TME2UMean8"/>
          </div>
          <div class="ui-block-b">
            Upper Median : <xsl:value-of select="@TME2UMedian8"/>
          </div>
          <div class="ui-block-a">
            Upper Variance : <xsl:value-of select="@TME2UVariance8"/>
          </div>
          <div class="ui-block-b">    
            Upper Std Dev : <xsl:value-of select="@TME2UStandDev8"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description8" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name9 != '' and @TME2Name9 != 'none')">
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
            Most Likely : <xsl:value-of select="@TME2TMAmount9"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2TMUnit9"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2MMean9"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2MMedian9"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2MVariance9"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2MStandDev9"/>
          </div>
          <div class="ui-block-a">
            Lower Total : <xsl:value-of select="@TME2TLAmount9"/>
          </div>
          <div class="ui-block-b">
            Lower Unit : <xsl:value-of select="@TME2TLUnit9"/>    
          </div>
          <div class="ui-block-a">
            Lower Mean : <xsl:value-of select="@TME2LMean9"/>
          </div>
          <div class="ui-block-b">
            Lower Median : <xsl:value-of select="@TME2LMedian9"/>
          </div>
          <div class="ui-block-a">
            Lower Variance : <xsl:value-of select="@TME2LVariance9"/>
          </div>
          <div class="ui-block-b">    
            Lower Std Dev : <xsl:value-of select="@TME2LStandDev9"/>
          </div>
          <div class="ui-block-a">
            Upper Total : <xsl:value-of select="@TME2TUAmount9"/>
          </div>
          <div class="ui-block-b">
            Upper Unit : <xsl:value-of select="@TME2TUUnit9"/>    
          </div>
          <div class="ui-block-a">
            Upper Mean : <xsl:value-of select="@TME2UMean9"/>
          </div>
          <div class="ui-block-b">
            Upper Median : <xsl:value-of select="@TME2UMedian9"/>
          </div>
          <div class="ui-block-a">
            Upper Variance : <xsl:value-of select="@TME2UVariance9"/>
          </div>
          <div class="ui-block-b">    
            Upper Std Dev : <xsl:value-of select="@TME2UStandDev9"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description9" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name10 != '' and @TME2Name10 != 'none')">
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
            Most Likely : <xsl:value-of select="@TME2TMAmount10"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2TMUnit10"/>
          </div>
          <div class="ui-block-a">
            Mean : <xsl:value-of select="@TME2MMean10"/>
          </div>
          <div class="ui-block-b">
            Median : <xsl:value-of select="@TME2MMedian10"/>
          </div>
          <div class="ui-block-a">
            Variance : <xsl:value-of select="@TME2MVariance10"/>
          </div>
          <div class="ui-block-b">    
            Std Dev : <xsl:value-of select="@TME2MStandDev10"/>
          </div>
          <div class="ui-block-a">
            Lower Total : <xsl:value-of select="@TME2TLAmount10"/>
          </div>
          <div class="ui-block-b">
            Lower Unit : <xsl:value-of select="@TME2TLUnit10"/>    
          </div>
          <div class="ui-block-a">
            Lower Mean : <xsl:value-of select="@TME2LMean10"/>
          </div>
          <div class="ui-block-b">
            Lower Median : <xsl:value-of select="@TME2LMedian10"/>
          </div>
          <div class="ui-block-a">
            Lower Variance : <xsl:value-of select="@TME2LVariance10"/>
          </div>
          <div class="ui-block-b">    
            Lower Std Dev : <xsl:value-of select="@TME2LStandDev10"/>
          </div>
          <div class="ui-block-a">
            Upper Total : <xsl:value-of select="@TME2TUAmount10"/>
          </div>
          <div class="ui-block-b">
            Upper Unit : <xsl:value-of select="@TME2TUUnit10"/>    
          </div>
          <div class="ui-block-a">
            Upper Mean : <xsl:value-of select="@TME2UMean10"/>
          </div>
          <div class="ui-block-b">
            Upper Median : <xsl:value-of select="@TME2UMedian10"/>
          </div>
          <div class="ui-block-a">
            Upper Variance : <xsl:value-of select="@TME2UVariance10"/>
          </div>
          <div class="ui-block-b">    
            Upper Std Dev : <xsl:value-of select="@TME2UStandDev10"/>
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description10" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name11 != '' and @TME2Name11 != 'none')">
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 1 Name : <strong><xsl:value-of select="@TME2Name11"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label11"/>
        </div>
        <div class="ui-block-a">
          Observations : <xsl:value-of select="@TME2N11"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Most Likely : <xsl:value-of select="@TME2TMAmount11"/>
        </div>
        <div class="ui-block-b">
          Unit : <xsl:value-of select="@TME2TMUnit11"/>
        </div>
        <div class="ui-block-a">
          Mean : <xsl:value-of select="@TME2MMean11"/>
        </div>
        <div class="ui-block-b">
          Median : <xsl:value-of select="@TME2MMedian11"/>
        </div>
        <div class="ui-block-a">
          Variance : <xsl:value-of select="@TME2MVariance11"/>
        </div>
        <div class="ui-block-b">    
          Std Dev : <xsl:value-of select="@TME2MStandDev11"/>
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount11"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TLUnit11"/>    
        </div>
        <div class="ui-block-a">
          Lower Mean : <xsl:value-of select="@TME2LMean11"/>
        </div>
        <div class="ui-block-b">
          Lower Median : <xsl:value-of select="@TME2LMedian11"/>
        </div>
        <div class="ui-block-a">
          Lower Variance : <xsl:value-of select="@TME2LVariance11"/>
        </div>
        <div class="ui-block-b">    
          Lower Std Dev : <xsl:value-of select="@TME2LStandDev11"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount11"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit11"/>    
        </div>
        <div class="ui-block-a">
          Upper Mean : <xsl:value-of select="@TME2UMean11"/>
        </div>
        <div class="ui-block-b">
          Upper Median : <xsl:value-of select="@TME2UMedian11"/>
        </div>
        <div class="ui-block-a">
          Upper Variance : <xsl:value-of select="@TME2UVariance11"/>
        </div>
        <div class="ui-block-b">    
          Upper Std Dev : <xsl:value-of select="@TME2UStandDev11"/>
        </div>
      </div>
      <div >
			  <strong>Description : </strong><xsl:value-of select="@TME2Description11" />
	    </div>
    </xsl:if>
    <xsl:if test="(@TME2Name12 != '' and @TME2Name12 != 'none')">
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 1 Name : <strong><xsl:value-of select="@TME2Name12"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label12"/>
        </div>
        <div class="ui-block-a">
          Observations : <xsl:value-of select="@TME2N12"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Most Likely : <xsl:value-of select="@TME2TMAmount12"/>
        </div>
        <div class="ui-block-b">
          Unit : <xsl:value-of select="@TME2TMUnit12"/>
        </div>
        <div class="ui-block-a">
          Mean : <xsl:value-of select="@TME2MMean12"/>
        </div>
        <div class="ui-block-b">
          Median : <xsl:value-of select="@TME2MMedian12"/>
        </div>
        <div class="ui-block-a">
          Variance : <xsl:value-of select="@TME2MVariance12"/>
        </div>
        <div class="ui-block-b">    
          Std Dev : <xsl:value-of select="@TME2MStandDev12"/>
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount12"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TLUnit12"/>    
        </div>
        <div class="ui-block-a">
          Lower Mean : <xsl:value-of select="@TME2LMean12"/>
        </div>
        <div class="ui-block-b">
          Lower Median : <xsl:value-of select="@TME2LMedian12"/>
        </div>
        <div class="ui-block-a">
          Lower Variance : <xsl:value-of select="@TME2LVariance12"/>
        </div>
        <div class="ui-block-b">    
          Lower Std Dev : <xsl:value-of select="@TME2LStandDev12"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount12"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit12"/>    
        </div>
        <div class="ui-block-a">
          Upper Mean : <xsl:value-of select="@TME2UMean12"/>
        </div>
        <div class="ui-block-b">
          Upper Median : <xsl:value-of select="@TME2UMedian12"/>
        </div>
        <div class="ui-block-a">
          Upper Variance : <xsl:value-of select="@TME2UVariance12"/>
        </div>
        <div class="ui-block-b">    
          Upper Std Dev : <xsl:value-of select="@TME2UStandDev12"/>
        </div>
      </div>
      <div >
			  <strong>Description : </strong><xsl:value-of select="@TME2Description12" />
	    </div>
    </xsl:if>
    <xsl:if test="(@TME2Name13 != '' and @TME2Name13 != 'none')">
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 1 Name : <strong><xsl:value-of select="@TME2Name13"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label13"/>
        </div>
        <div class="ui-block-a">
          Observations : <xsl:value-of select="@TME2N13"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Most Likely : <xsl:value-of select="@TME2TMAmount13"/>
        </div>
        <div class="ui-block-b">
          Unit : <xsl:value-of select="@TME2TMUnit13"/>
        </div>
        <div class="ui-block-a">
          Mean : <xsl:value-of select="@TME2MMean13"/>
        </div>
        <div class="ui-block-b">
          Median : <xsl:value-of select="@TME2MMedian13"/>
        </div>
        <div class="ui-block-a">
          Variance : <xsl:value-of select="@TME2MVariance13"/>
        </div>
        <div class="ui-block-b">    
          Std Dev : <xsl:value-of select="@TME2MStandDev13"/>
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount13"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TLUnit13"/>    
        </div>
        <div class="ui-block-a">
          Lower Mean : <xsl:value-of select="@TME2LMean13"/>
        </div>
        <div class="ui-block-b">
          Lower Median : <xsl:value-of select="@TME2LMedian13"/>
        </div>
        <div class="ui-block-a">
          Lower Variance : <xsl:value-of select="@TME2LVariance13"/>
        </div>
        <div class="ui-block-b">    
          Lower Std Dev : <xsl:value-of select="@TME2LStandDev13"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount13"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit13"/>    
        </div>
        <div class="ui-block-a">
          Upper Mean : <xsl:value-of select="@TME2UMean13"/>
        </div>
        <div class="ui-block-b">
          Upper Median : <xsl:value-of select="@TME2UMedian13"/>
        </div>
        <div class="ui-block-a">
          Upper Variance : <xsl:value-of select="@TME2UVariance13"/>
        </div>
        <div class="ui-block-b">    
          Upper Std Dev : <xsl:value-of select="@TME2UStandDev13"/>
        </div>
      </div>
      <div >
			  <strong>Description : </strong><xsl:value-of select="@TME2Description13" />
	    </div>
    </xsl:if>
    <xsl:if test="(@TME2Name14 != '' and @TME2Name14 != 'none')">
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 1 Name : <strong><xsl:value-of select="@TME2Name14"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label14"/>
        </div>
        <div class="ui-block-a">
          Observations : <xsl:value-of select="@TME2N14"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Most Likely : <xsl:value-of select="@TME2TMAmount14"/>
        </div>
        <div class="ui-block-b">
          Unit : <xsl:value-of select="@TME2TMUnit14"/>
        </div>
        <div class="ui-block-a">
          Mean : <xsl:value-of select="@TME2MMean14"/>
        </div>
        <div class="ui-block-b">
          Median : <xsl:value-of select="@TME2MMedian14"/>
        </div>
        <div class="ui-block-a">
          Variance : <xsl:value-of select="@TME2MVariance14"/>
        </div>
        <div class="ui-block-b">    
          Std Dev : <xsl:value-of select="@TME2MStandDev14"/>
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount14"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TLUnit14"/>    
        </div>
        <div class="ui-block-a">
          Lower Mean : <xsl:value-of select="@TME2LMean14"/>
        </div>
        <div class="ui-block-b">
          Lower Median : <xsl:value-of select="@TME2LMedian14"/>
        </div>
        <div class="ui-block-a">
          Lower Variance : <xsl:value-of select="@TME2LVariance14"/>
        </div>
        <div class="ui-block-b">    
          Lower Std Dev : <xsl:value-of select="@TME2LStandDev14"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount14"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit14"/>    
        </div>
        <div class="ui-block-a">
          Upper Mean : <xsl:value-of select="@TME2UMean14"/>
        </div>
        <div class="ui-block-b">
          Upper Median : <xsl:value-of select="@TME2UMedian14"/>
        </div>
        <div class="ui-block-a">
          Upper Variance : <xsl:value-of select="@TME2UVariance14"/>
        </div>
        <div class="ui-block-b">    
          Upper Std Dev : <xsl:value-of select="@TME2UStandDev14"/>
        </div>
      </div>
      <div >
			  <strong>Description : </strong><xsl:value-of select="@TME2Description14" />
	    </div>
    </xsl:if>
    <xsl:if test="(@TME2Name15 != '' and @TME2Name15 != 'none')">
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 1 Name : <strong><xsl:value-of select="@TME2Name15"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label15"/>
        </div>
        <div class="ui-block-a">
          Observations : <xsl:value-of select="@TME2N15"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Most Likely : <xsl:value-of select="@TME2TMAmount15"/>
        </div>
        <div class="ui-block-b">
          Unit : <xsl:value-of select="@TME2TMUnit15"/>
        </div>
        <div class="ui-block-a">
          Mean : <xsl:value-of select="@TME2MMean15"/>
        </div>
        <div class="ui-block-b">
          Median : <xsl:value-of select="@TME2MMedian15"/>
        </div>
        <div class="ui-block-a">
          Variance : <xsl:value-of select="@TME2MVariance15"/>
        </div>
        <div class="ui-block-b">    
          Std Dev : <xsl:value-of select="@TME2MStandDev15"/>
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount15"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TLUnit15"/>    
        </div>
        <div class="ui-block-a">
          Lower Mean : <xsl:value-of select="@TME2LMean15"/>
        </div>
        <div class="ui-block-b">
          Lower Median : <xsl:value-of select="@TME2LMedian15"/>
        </div>
        <div class="ui-block-a">
          Lower Variance : <xsl:value-of select="@TME2LVariance15"/>
        </div>
        <div class="ui-block-b">    
          Lower Std Dev : <xsl:value-of select="@TME2LStandDev15"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount15"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit15"/>    
        </div>
        <div class="ui-block-a">
          Upper Mean : <xsl:value-of select="@TME2UMean15"/>
        </div>
        <div class="ui-block-b">
          Upper Median : <xsl:value-of select="@TME2UMedian15"/>
        </div>
        <div class="ui-block-a">
          Upper Variance : <xsl:value-of select="@TME2UVariance15"/>
        </div>
        <div class="ui-block-b">    
          Upper Std Dev : <xsl:value-of select="@TME2UStandDev15"/>
        </div>
      </div>
      <div >
			  <strong>Description : </strong><xsl:value-of select="@TME2Description15" />
	    </div>
    </xsl:if>
    </div>
	</xsl:template>
</xsl:stylesheet>

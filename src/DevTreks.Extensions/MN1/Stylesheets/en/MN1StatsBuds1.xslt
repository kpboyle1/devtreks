<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, June -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Budget"
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
			<xsl:apply-templates select="budgetgroup" />
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
		<xsl:apply-templates select="budgetgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetgroup">
    <h4>
      <strong>Budget Group</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budget">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budget">
		<h4>
      <strong>Budget</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgettimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgettimeperiod">
		<h4>
      <strong>Time Period</strong> : <xsl:value-of select="@Name" />
    </h4>
    <xsl:apply-templates select="root/linkedview[@AnalyzerType='mnstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <div>
      <strong>Outcomes</strong>
    </div>
		<xsl:apply-templates select="budgetoutcomes" />
    <div>
      <strong>Operations</strong>
    </div>
		<xsl:apply-templates select="budgetoperations" />
	</xsl:template>
	<xsl:template match="budgetoutcomes">
		<xsl:apply-templates select="budgetoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutcome">
		<h4>
      <strong>Outcome </strong> : <xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgetoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutput">
    <h4>
      <strong>Output </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoperations">
		<xsl:apply-templates select="budgetoperation">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoperation">
		<h4>
      <strong>Operation</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgetinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetinput">
		<h4>
      <strong>Input </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Nutrient Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Q Observations : <xsl:value-of select="@TQN"/>
        </div>
        <div class="ui-block-b">
            
        </div>
        <div class="ui-block-a">
          MN1 Name : <xsl:value-of select="@TMN1Name"/>
        </div>
        <div class="ui-block-b">
          MN1 Total : <xsl:value-of select="@TMN1Q"/>
        </div>
        <div class="ui-block-a">
          MN1 Mean : <xsl:value-of select="@TMN1Mean"/>
        </div>
        <div class="ui-block-b">
          MN1 Median : <xsl:value-of select="@TMN1Median"/>
        </div>
        <div class="ui-block-a">
          MN1 Variance : <xsl:value-of select="@TMN1Variance"/>
        </div>
        <div class="ui-block-b">  
          MN1 Std Dev : <xsl:value-of select="@TMN1StandDev"/>
        </div>
        <div class="ui-block-a">
          MN2 Name : <xsl:value-of select="@TMN2Name"/>
        </div>
        <div class="ui-block-b">
          MN2 Total : <xsl:value-of select="@TMN2Q"/>
        </div>
        <div class="ui-block-a">
          MN2 Mean : <xsl:value-of select="@TMN2Mean"/>
        </div>
        <div class="ui-block-b">
          MN2 Median : <xsl:value-of select="@TMN2Median"/>
        </div>
        <div class="ui-block-a">
          MN2 Variance : <xsl:value-of select="@TMN2Variance"/>
        </div>
        <div class="ui-block-b">  
          MN2 Std Dev : <xsl:value-of select="@TMN2StandDev"/>
        </div>
        <xsl:if test="(string-length(@TMN3Name) > 0)">
          <div class="ui-block-a">
            MN3 Name : <xsl:value-of select="@TMN3Name"/>
          </div>
          <div class="ui-block-b">
            MN3 Total : <xsl:value-of select="@TMN3Q"/>
          </div>
          <div class="ui-block-a">
            MN3 Mean : <xsl:value-of select="@TMN3Mean"/>
          </div>
          <div class="ui-block-b">
            MN3 Median : <xsl:value-of select="@TMN3Median"/>
          </div>
          <div class="ui-block-a">
            MN3 Variance : <xsl:value-of select="@TMN3Variance"/>
          </div>
          <div class="ui-block-b">  
            MN3 Std Dev : <xsl:value-of select="@TMN3StandDev"/>
          </div>
        </xsl:if>
        <xsl:if test="(string-length(@TMN4Name) > 0)">
          <div class="ui-block-a">
            MN4 Name : <xsl:value-of select="@TMN4Name"/>
          </div>
          <div class="ui-block-b">
            MN4 Total : <xsl:value-of select="@TMN4Q"/>
          </div>
          <div class="ui-block-a">
            MN4 Mean : <xsl:value-of select="@TMN4Mean"/>
          </div>
          <div class="ui-block-b">
            MN4 Median : <xsl:value-of select="@TMN4Median"/>
          </div>
          <div class="ui-block-a">
            MN4 Variance : <xsl:value-of select="@TMN4Variance"/>
          </div>
          <div class="ui-block-b">  
            MN4 Std Dev : <xsl:value-of select="@TMN4StandDev"/>
          </div>
        </xsl:if>
        <xsl:if test="(string-length(@TMN5Name) > 0)">
          <div class="ui-block-a">
            MN5 Name : <xsl:value-of select="@TMN5Name"/>
          </div>
          <div class="ui-block-b">
            MN5 Total : <xsl:value-of select="@TMN5Q"/>
          </div>
          <div class="ui-block-a">
            MN5 Mean : <xsl:value-of select="@TMN5Mean"/>
          </div>
          <div class="ui-block-b">
            MN5 Median : <xsl:value-of select="@TMN5Median"/>
          </div>
          <div class="ui-block-a">
            MN5 Variance : <xsl:value-of select="@TMN5Variance"/>
          </div>
          <div class="ui-block-b">  
            MN5 Std Dev : <xsl:value-of select="@TMN5StandDev"/>
          </div>
        </xsl:if>
        <xsl:if test="(string-length(@TMN6Name) > 0)">
          <div class="ui-block-a">
            MN6 Name : <xsl:value-of select="@TMN6Name"/>
          </div>
          <div class="ui-block-b">
            MN6 Total : <xsl:value-of select="@TMN6Q"/>
          </div>
          <div class="ui-block-a">
            MN6 Mean : <xsl:value-of select="@TMN6Mean"/>
          </div>
          <div class="ui-block-b">
            MN6 Median : <xsl:value-of select="@TMN6Median"/>
          </div>
          <div class="ui-block-a">
            MN6 Variance : <xsl:value-of select="@TMN6Variance"/>
          </div>
          <div class="ui-block-b">  
              MN6 Std Dev : <xsl:value-of select="@TMN6StandDev"/>
          </div>
        </xsl:if>
        <xsl:if test="(string-length(@TMN7Name) > 0)">
          <div class="ui-block-a">
            MN7 Name : <xsl:value-of select="@TMN7Name"/>
          </div>
          <div class="ui-block-b">
            MN7 Total : <xsl:value-of select="@TMN7Q"/>
          </div>
          <div class="ui-block-a">
            MN7 Mean : <xsl:value-of select="@TMN7Mean"/>
          </div>
          <div class="ui-block-b">
            MN7 Median : <xsl:value-of select="@TMN7Median"/>
          </div>
          <div class="ui-block-a">
            MN7 Variance : <xsl:value-of select="@TMN7Variance"/>
          </div>
          <div class="ui-block-b">  
              MN7 Std Dev : <xsl:value-of select="@TMN7StandDev"/>
          </div>
        </xsl:if>
        <xsl:if test="(string-length(@TMN8Name) > 0)">
          <div class="ui-block-a">
            MN8 Name : <xsl:value-of select="@TMN8Name"/>
          </div>
          <div class="ui-block-b">
            MN8 Total : <xsl:value-of select="@TMN8Q"/>
          </div>
          <div class="ui-block-a">
            MN8 Mean : <xsl:value-of select="@TMN8Mean"/>
          </div>
          <div class="ui-block-b">
            MN8 Median : <xsl:value-of select="@TMN8Median"/>
          </div>
          <div class="ui-block-a">
            MN8 Variance : <xsl:value-of select="@TMN8Variance"/>
          </div>
          <div class="ui-block-b">  
              MN8 Std Dev : <xsl:value-of select="@TMN8StandDev"/>
          </div>
         </xsl:if>
        <xsl:if test="(string-length(@TMN9Name) > 0)">
          <div class="ui-block-a">
            MN9 Name : <xsl:value-of select="@TMN9Name"/>
          </div>
          <div class="ui-block-b">
            MN9 Total : <xsl:value-of select="@TMN9Q"/>
          </div>
          <div class="ui-block-a">
            MN9 Mean : <xsl:value-of select="@TMN9Mean"/>
          </div>
          <div class="ui-block-b">
            MN9 Median : <xsl:value-of select="@TMN9Median"/>
          </div>
          <div class="ui-block-a">
            MN9 Variance : <xsl:value-of select="@TMN9Variance"/>
          </div>
          <div class="ui-block-b">  
              MN9 Std Dev : <xsl:value-of select="@TMN9StandDev"/>
          </div>
        </xsl:if>
        <xsl:if test="(string-length(@TMN10Name) > 0)">
          <div class="ui-block-a">
            MN10 Name : <xsl:value-of select="@TMN10Name"/>
          </div>
          <div class="ui-block-b">
            MN10 Total : <xsl:value-of select="@TMN10Q"/>
          </div>
          <div class="ui-block-a">
            MN10 Mean : <xsl:value-of select="@TMN10Mean"/>
          </div>
          <div class="ui-block-b">
            MN10 Median : <xsl:value-of select="@TMN10Median"/>
          </div>
          <div class="ui-block-a">
            MN10 Variance : <xsl:value-of select="@TMN10Variance"/>
          </div>
          <div class="ui-block-b">  
              MN10 Std Dev : <xsl:value-of select="@TMN10StandDev"/>
          </div>
        </xsl:if>
      </div>
    </div>
	</xsl:template>
</xsl:stylesheet>

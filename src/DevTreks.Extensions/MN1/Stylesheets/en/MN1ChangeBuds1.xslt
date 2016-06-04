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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnchangeyr' 
      or @AnalyzerType='mnchangeid' or @AnalyzerType='mnchangealt']">
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnchangeyr' 
      or @AnalyzerType='mnchangeid' or @AnalyzerType='mnchangealt']">
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
    <xsl:apply-templates select="root/linkedview[@AnalyzerType='mnchangeyr' 
      or @AnalyzerType='mnchangeid' or @AnalyzerType='mnchangealt']">
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnchangeyr' 
      or @AnalyzerType='mnchangeid' or @AnalyzerType='mnchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgetoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutput">
    <h4>
      <strong>Output </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnchangeyr' 
      or @AnalyzerType='mnchangeid' or @AnalyzerType='mnchangealt']">
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnchangeyr' 
      or @AnalyzerType='mnchangeid' or @AnalyzerType='mnchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgetinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetinput">
		<h4>
      <strong>Input </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnchangeyr' 
      or @AnalyzerType='mnchangeid' or @AnalyzerType='mnchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Nutrient Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Date : <xsl:value-of select="@Date"/>
          </div>
          <div class="ui-block-b">
            Observations : <xsl:value-of select="@Observations"/>; Alternative : <xsl:value-of select="@AlternativeType"/>
          </div>
          <div class="ui-block-a">
            Name : <xsl:value-of select="@TMN1Name"/>
          </div>
          <div class="ui-block-b">
            Total : <xsl:value-of select="@TMN1Q"/>
          </div>
          <div class="ui-block-a">
            Amount Change : <xsl:value-of select="@TMN1AmountChange"/>
          </div>
          <div class="ui-block-b">
            Percent Change : <xsl:value-of select="@TMN1PercentChange"/>
          </div>
          <div class="ui-block-a">
            Base Change : <xsl:value-of select="@TMN1BaseChange"/>
          </div>
          <div class="ui-block-b">   
            Base Percent Change : <xsl:value-of select="@TMN1BasePercentChange"/>
          </div>
          <div class="ui-block-a">
            Name : <xsl:value-of select="@TMN2Name"/>
          </div>
          <div class="ui-block-b">
            Total : <xsl:value-of select="@TMN2Q"/>
          </div>
          <div class="ui-block-a">
            Amount Change : <xsl:value-of select="@TMN2AmountChange"/>
          </div>
          <div class="ui-block-b">
            Percent Change : <xsl:value-of select="@TMN2PercentChange"/>
          </div>
          <div class="ui-block-a">
            Base Change : <xsl:value-of select="@TMN2BaseChange"/>
          </div>
          <div class="ui-block-b">   
            Base Percent Change : <xsl:value-of select="@TMN2BasePercentChange"/>
          </div>
          <xsl:if test="(string-length(@TMN3Name) > 0)">
            <div class="ui-block-a">
              Name : <xsl:value-of select="@TMN3Name"/>
            </div>
            <div class="ui-block-b">
              Total : <xsl:value-of select="@TMN3Q"/>
            </div>
            <div class="ui-block-a">
              Amount Change : <xsl:value-of select="@TMN3AmountChange"/>
            </div>
            <div class="ui-block-b">
              Percent Change : <xsl:value-of select="@TMN3PercentChange"/>
            </div>
            <div class="ui-block-a">
              Base Change : <xsl:value-of select="@TMN3BaseChange"/>
            </div>
            <div class="ui-block-b">   
              Base Percent Change : <xsl:value-of select="@TMN3BasePercentChange"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TMN4Name) > 0)">
            <div class="ui-block-a">
              Name : <xsl:value-of select="@TMN4Name"/>
            </div>
            <div class="ui-block-b">
              Total : <xsl:value-of select="@TMN4Q"/>
            </div>
            <div class="ui-block-a">
              Amount Change : <xsl:value-of select="@TMN4AmountChange"/>
            </div>
            <div class="ui-block-b">
              Percent Change : <xsl:value-of select="@TMN4PercentChange"/>
            </div>
            <div class="ui-block-a">
              Base Change : <xsl:value-of select="@TMN4BaseChange"/>
            </div>
            <div class="ui-block-b">   
              Base Percent Change : <xsl:value-of select="@TMN4BasePercentChange"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TMN5Name) > 0)">
            <div class="ui-block-a">
              Name : <xsl:value-of select="@TMN5Name"/>
            </div>
            <div class="ui-block-b">
              Total : <xsl:value-of select="@TMN5Q"/>
            </div>
            <div class="ui-block-a">
              Amount Change : <xsl:value-of select="@TMN5AmountChange"/>
            </div>
            <div class="ui-block-b">
              Percent Change : <xsl:value-of select="@TMN5PercentChange"/>
            </div>
            <div class="ui-block-a">
              Base Change : <xsl:value-of select="@TMN5BaseChange"/>
            </div>
            <div class="ui-block-b">   
              Base Percent Change : <xsl:value-of select="@TMN5BasePercentChange"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TMN6Name) > 0)">
            <div class="ui-block-a">
              Name : <xsl:value-of select="@TMN6Name"/>
            </div>
            <div class="ui-block-b">
              Total : <xsl:value-of select="@TMN6Q"/>
            </div>
            <div class="ui-block-a">
              Amount Change : <xsl:value-of select="@TMN6AmountChange"/>
            </div>
            <div class="ui-block-b">
              Percent Change : <xsl:value-of select="@TMN6PercentChange"/>
            </div>
            <div class="ui-block-a">
              Base Change : <xsl:value-of select="@TMN6BaseChange"/>
            </div>
            <div class="ui-block-b">   
              Base Percent Change : <xsl:value-of select="@TMN6BasePercentChange"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TMN7Name) > 0)">
            <div class="ui-block-a">
              Name : <xsl:value-of select="@TMN7Name"/>
            </div>
            <div class="ui-block-b">
              Total : <xsl:value-of select="@TMN7Q"/>
            </div>
            <div class="ui-block-a">
              Amount Change : <xsl:value-of select="@TMN7AmountChange"/>
            </div>
            <div class="ui-block-b">
              Percent Change : <xsl:value-of select="@TMN7PercentChange"/>
            </div>
            <div class="ui-block-a">
              Base Change : <xsl:value-of select="@TMN7BaseChange"/>
            </div>
            <div class="ui-block-b">   
              Base Percent Change : <xsl:value-of select="@TMN7BasePercentChange"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TMN8Name) > 0)">
            <div class="ui-block-a">
              Name : <xsl:value-of select="@TMN8Name"/>
            </div>
            <div class="ui-block-b">
              Total : <xsl:value-of select="@TMN8Q"/>
            </div>
            <div class="ui-block-a">
              Amount Change : <xsl:value-of select="@TMN8AmountChange"/>
            </div>
            <div class="ui-block-b">
              Percent Change : <xsl:value-of select="@TMN8PercentChange"/>
            </div>
            <div class="ui-block-a">
              Base Change : <xsl:value-of select="@TMN8BaseChange"/>
            </div>
            <div class="ui-block-b">   
              Base Percent Change : <xsl:value-of select="@TMN8BasePercentChange"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TMN9Name) > 0)">
            <div class="ui-block-a">
              Name : <xsl:value-of select="@TMN9Name"/>
            </div>
            <div class="ui-block-b">
              Total : <xsl:value-of select="@TMN9Q"/>
            </div>
            <div class="ui-block-a">
              Amount Change : <xsl:value-of select="@TMN9AmountChange"/>
            </div>
            <div class="ui-block-b">
              Percent Change : <xsl:value-of select="@TMN9PercentChange"/>
            </div>
            <div class="ui-block-a">
              Base Change : <xsl:value-of select="@TMN9BaseChange"/>
            </div>
            <div class="ui-block-b">   
              Base Percent Change : <xsl:value-of select="@TMN9BasePercentChange"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TMN10Name) > 0)">
            <div class="ui-block-a">
              Name : <xsl:value-of select="@TMN10Name"/>
            </div>
            <div class="ui-block-b">
              Total : <xsl:value-of select="@TMN10Q"/>
            </div>
            <div class="ui-block-a">
              Amount Change : <xsl:value-of select="@TMN10AmountChange"/>
            </div>
            <div class="ui-block-b">
              Percent Change : <xsl:value-of select="@TMN10PercentChange"/>
            </div>
            <div class="ui-block-a">
              Base Change : <xsl:value-of select="@TMN10BaseChange"/>
            </div>
            <div class="ui-block-b">   
              Base Percent Change : <xsl:value-of select="@TMN10BasePercentChange"/>
            </div>
          </xsl:if>
        </div>
      </div>
	</xsl:template>
</xsl:stylesheet>

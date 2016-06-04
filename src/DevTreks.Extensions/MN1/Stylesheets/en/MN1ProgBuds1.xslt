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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnprogress1']">
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgettimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgettimeperiod">
		<h4>
      <strong>Time Period</strong> : <xsl:value-of select="@Name" />Label:&#xA0;<xsl:value-of select="@Num" />
    </h4>
    <xsl:apply-templates select="root/linkedview[@AnalyzerType='mnprogress1']">
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
      <strong>Outcome </strong>: <xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" />;&#xA0;Label:&#xA0;<xsl:value-of select="@Num" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgetoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutput">
    <h4>
      <strong>Output </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnprogress1']">
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
      <strong>Operation</strong>: <xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" />;&#xA0;Label:&#xA0;<xsl:value-of select="@Num" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgetinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetinput">
		<h4>
      <strong>Input </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnprogress1']">
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
            Observations : <xsl:value-of select="@Observations"/>
          </div>
          <div class="ui-block-a">
            Target Type : <xsl:value-of select="@TargetType"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Name : <xsl:value-of select="@TMN1Name"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Planned Period : <xsl:value-of select="@TMN1Q"/>
          </div>
          <div class="ui-block-b">
            Plan Full : <xsl:value-of select="@TMN1PFTotal"/>
          </div>
          <div class="ui-block-a">
            Plan Cumul : <xsl:value-of select="@TMN1PCTotal"/>
          </div>
          <div class="ui-block-b">
            Actual Period : <xsl:value-of select="@TMN1APTotal"/>
          </div>
          <div class="ui-block-a">
            Actual Cumul : <xsl:value-of select="@TMN1ACTotal"/>
          </div>
          <div class="ui-block-b">
            Actual Period Change : <xsl:value-of select="@TMN1APChange"/>
          </div>
          <div class="ui-block-a">
            Actual Cumul Change : <xsl:value-of select="@TMN1ACChange"/>
          </div>
          <div class="ui-block-b">
            Planned Period Percent : <xsl:value-of select="@TMN1PPPercent"/>
          </div>
          <div class="ui-block-a">
            Planned Cumul Percent : <xsl:value-of select="@TMN1PCPercent"/>
          </div>
          <div class="ui-block-b">
            Planned Full Percent : <xsl:value-of select="@TMN1PFPercent"/>
          </div>
          <div class="ui-block-a">
            Name : <xsl:value-of select="@TMN2Name"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Planned Period : <xsl:value-of select="@TMN2Q"/>
          </div>
          <div class="ui-block-b">
            Plan Full : <xsl:value-of select="@TMN2PFTotal"/>
          </div>
          <div class="ui-block-a">
            Plan Cumul : <xsl:value-of select="@TMN2PCTotal"/>
          </div>
          <div class="ui-block-b">
            Actual Period : <xsl:value-of select="@TMN2APTotal"/>
          </div>
          <div class="ui-block-a">
            Actual Cumul : <xsl:value-of select="@TMN2ACTotal"/>
          </div>
          <div class="ui-block-b">
            Actual Period Change : <xsl:value-of select="@TMN2APChange"/>
          </div>
          <div class="ui-block-a">
            Actual Cumul Change : <xsl:value-of select="@TMN2ACChange"/>
          </div>
          <div class="ui-block-b">
            Planned Period Percent : <xsl:value-of select="@TMN2PPPercent"/>
          </div>
          <div class="ui-block-a">
            Planned Cumul Percent : <xsl:value-of select="@TMN2PCPercent"/>
          </div>
          <div class="ui-block-b">
            Planned Full Percent : <xsl:value-of select="@TMN2PFPercent"/>
          </div>
          <xsl:if test="(string-length(@TMN3Name) > 0)">
            <div class="ui-block-a">
              Name : <xsl:value-of select="@TMN3Name"/>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Planned Period : <xsl:value-of select="@TMN3Q"/>
            </div>
            <div class="ui-block-b">
              Plan Full : <xsl:value-of select="@TMN3PFTotal"/>
            </div>
            <div class="ui-block-a">
              Plan Cumul : <xsl:value-of select="@TMN3PCTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period : <xsl:value-of select="@TMN3APTotal"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul : <xsl:value-of select="@TMN3ACTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period Change : <xsl:value-of select="@TMN3APChange"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul Change : <xsl:value-of select="@TMN3ACChange"/>
            </div>
            <div class="ui-block-b">
              Planned Period Percent : <xsl:value-of select="@TMN3PPPercent"/>
            </div>
            <div class="ui-block-a">
              Planned Cumul Percent : <xsl:value-of select="@TMN3PCPercent"/>
            </div>
            <div class="ui-block-b">
              Planned Full Percent : <xsl:value-of select="@TMN3PFPercent"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TMN4Name) > 0)">
            <div class="ui-block-a">
              Name : <xsl:value-of select="@TMN4Name"/>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Planned Period : <xsl:value-of select="@TMN4Q"/>
            </div>
            <div class="ui-block-b">
              Plan Full : <xsl:value-of select="@TMN4PFTotal"/>
            </div>
            <div class="ui-block-a">
              Plan Cumul : <xsl:value-of select="@TMN4PCTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period : <xsl:value-of select="@TMN4APTotal"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul : <xsl:value-of select="@TMN4ACTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period Change : <xsl:value-of select="@TMN4APChange"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul Change : <xsl:value-of select="@TMN4ACChange"/>
            </div>
            <div class="ui-block-b">
              Planned Period Percent : <xsl:value-of select="@TMN4PPPercent"/>
            </div>
            <div class="ui-block-a">
              Planned Cumul Percent : <xsl:value-of select="@TMN4PCPercent"/>
            </div>
            <div class="ui-block-b">
              Planned Full Percent : <xsl:value-of select="@TMN4PFPercent"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TMN5Name) > 0)">
            <div class="ui-block-a">
              Name : <xsl:value-of select="@TMN5Name"/>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Planned Period : <xsl:value-of select="@TMN5Q"/>
            </div>
            <div class="ui-block-b">
              Plan Full : <xsl:value-of select="@TMN5PFTotal"/>
            </div>
            <div class="ui-block-a">
              Plan Cumul : <xsl:value-of select="@TMN5PCTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period : <xsl:value-of select="@TMN5APTotal"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul : <xsl:value-of select="@TMN5ACTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period Change : <xsl:value-of select="@TMN5APChange"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul Change : <xsl:value-of select="@TMN5ACChange"/>
            </div>
            <div class="ui-block-b">
              Planned Period Percent : <xsl:value-of select="@TMN5PPPercent"/>
            </div>
            <div class="ui-block-a">
              Planned Cumul Percent : <xsl:value-of select="@TMN5PCPercent"/>
            </div>
            <div class="ui-block-b">
              Planned Full Percent : <xsl:value-of select="@TMN5PFPercent"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TMN6Name) > 0)">
            <div class="ui-block-a">
              Name : <xsl:value-of select="@TMN6Name"/>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Planned Period : <xsl:value-of select="@TMN6Q"/>
            </div>
            <div class="ui-block-b">
              Plan Full : <xsl:value-of select="@TMN6PFTotal"/>
            </div>
            <div class="ui-block-a">
              Plan Cumul : <xsl:value-of select="@TMN6PCTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period : <xsl:value-of select="@TMN6APTotal"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul : <xsl:value-of select="@TMN6ACTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period Change : <xsl:value-of select="@TMN6APChange"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul Change : <xsl:value-of select="@TMN6ACChange"/>
            </div>
            <div class="ui-block-b">
              Planned Period Percent : <xsl:value-of select="@TMN6PPPercent"/>
            </div>
            <div class="ui-block-a">
              Planned Cumul Percent : <xsl:value-of select="@TMN6PCPercent"/>
            </div>
            <div class="ui-block-b">
              Planned Full Percent : <xsl:value-of select="@TMN6PFPercent"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TMN7Name) > 0)">
            <div class="ui-block-a">
              Name : <xsl:value-of select="@TMN7Name"/>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Planned Period : <xsl:value-of select="@TMN7Q"/>
            </div>
            <div class="ui-block-b">
              Plan Full : <xsl:value-of select="@TMN7PFTotal"/>
            </div>
            <div class="ui-block-a">
              Plan Cumul : <xsl:value-of select="@TMN7PCTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period : <xsl:value-of select="@TMN7APTotal"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul : <xsl:value-of select="@TMN7ACTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period Change : <xsl:value-of select="@TMN7APChange"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul Change : <xsl:value-of select="@TMN7ACChange"/>
            </div>
            <div class="ui-block-b">
              Planned Period Percent : <xsl:value-of select="@TMN7PPPercent"/>
            </div>
            <div class="ui-block-a">
              Planned Cumul Percent : <xsl:value-of select="@TMN7PCPercent"/>
            </div>
            <div class="ui-block-b">
              Planned Full Percent : <xsl:value-of select="@TMN7PFPercent"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TMN8Name) > 0)">
            <div class="ui-block-a">
              Name : <xsl:value-of select="@TMN8Name"/>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Planned Period : <xsl:value-of select="@TMN8Q"/>
            </div>
            <div class="ui-block-b">
              Plan Full : <xsl:value-of select="@TMN8PFTotal"/>
            </div>
            <div class="ui-block-a">
              Plan Cumul : <xsl:value-of select="@TMN8PCTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period : <xsl:value-of select="@TMN8APTotal"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul : <xsl:value-of select="@TMN8ACTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period Change : <xsl:value-of select="@TMN8APChange"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul Change : <xsl:value-of select="@TMN8ACChange"/>
            </div>
            <div class="ui-block-b">
              Planned Period Percent : <xsl:value-of select="@TMN8PPPercent"/>
            </div>
            <div class="ui-block-a">
              Planned Cumul Percent : <xsl:value-of select="@TMN8PCPercent"/>
            </div>
            <div class="ui-block-b">
              Planned Full Percent : <xsl:value-of select="@TMN8PFPercent"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TMN9Name) > 0)">
            <div class="ui-block-a">
              Name : <xsl:value-of select="@TMN9Name"/>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Planned Period : <xsl:value-of select="@TMN9Q"/>
            </div>
            <div class="ui-block-b">
              Plan Full : <xsl:value-of select="@TMN9PFTotal"/>
            </div>
            <div class="ui-block-a">
              Plan Cumul : <xsl:value-of select="@TMN9PCTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period : <xsl:value-of select="@TMN9APTotal"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul : <xsl:value-of select="@TMN9ACTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period Change : <xsl:value-of select="@TMN9APChange"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul Change : <xsl:value-of select="@TMN9ACChange"/>
            </div>
            <div class="ui-block-b">
              Planned Period Percent : <xsl:value-of select="@TMN9PPPercent"/>
            </div>
            <div class="ui-block-a">
              Planned Cumul Percent : <xsl:value-of select="@TMN9PCPercent"/>
            </div>
            <div class="ui-block-b">
              Planned Full Percent : <xsl:value-of select="@TMN9PFPercent"/>
            </div>
          </xsl:if>
          <xsl:if test="(string-length(@TMN10Name) > 0)">
            <div class="ui-block-a">
              Name : <xsl:value-of select="@TMN10Name"/>
            </div>
            <div class="ui-block-b">
            </div>
            <div class="ui-block-a">
              Planned Period : <xsl:value-of select="@TMN10Q"/>
            </div>
            <div class="ui-block-b">
              Plan Full : <xsl:value-of select="@TMN10PFTotal"/>
            </div>
            <div class="ui-block-a">
              Plan Cumul : <xsl:value-of select="@TMN10PCTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period : <xsl:value-of select="@TMN10APTotal"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul : <xsl:value-of select="@TMN10ACTotal"/>
            </div>
            <div class="ui-block-b">
              Actual Period Change : <xsl:value-of select="@TMN10APChange"/>
            </div>
            <div class="ui-block-a">
              Actual Cumul Change : <xsl:value-of select="@TMN10ACChange"/>
            </div>
            <div class="ui-block-b">
              Planned Period Percent : <xsl:value-of select="@TMN10PPPercent"/>
            </div>
            <div class="ui-block-a">
              Planned Cumul Percent : <xsl:value-of select="@TMN10PCPercent"/>
            </div>
            <div class="ui-block-b">
              Planned Full Percent : <xsl:value-of select="@TMN10PFPercent"/>
            </div>
          </xsl:if>
        </div>
     </div>
	</xsl:template>
</xsl:stylesheet>


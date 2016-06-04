<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, June -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Budget"
	xmlns:DisplayComps="urn:displaycomps">
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
      <strong>Budget Group</strong> : <xsl:value-of select="@Name" /> ; <xsl:value-of select="@Date_0" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budget">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budget">
		<xsl:variable name="fullcolcount" select="@Files + 1"/>
    <h4 class="ui-bar-b">
      Budget Name, Date, Label
    </h4>
		<xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
		<xsl:for-each select="@*">
			<xsl:variable name="att_name" select="name()"/>
			<xsl:variable name="att_value" select="."/>
			<xsl:value-of select="DisplayComps:printValue('Name_', $att_name, $att_value)"/> 
		</xsl:for-each>
    <div class="ui-grid-a">
			<xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
    </div>
    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
		<xsl:for-each select="@*">
			<xsl:variable name="att_name" select="name()"/>
			<xsl:variable name="att_value" select="."/>
			<xsl:value-of select="DisplayComps:printValue('Date_', $att_name, $att_value)"/> 
		</xsl:for-each>
    <div class="ui-grid-a">
			<xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
    </div>
    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
		<xsl:for-each select="@*">
			<xsl:variable name="att_name" select="name()"/>
			<xsl:variable name="att_value" select="."/>
			<xsl:value-of select="DisplayComps:printValue('Num_', $att_name, $att_value)"/> 
		</xsl:for-each>
    <div class="ui-grid-a">
			<xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
    </div>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgettimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgettimeperiod">
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
    <h4 class="ui-bar-b">
      Time Period Name, Date, Label
    </h4>
		<xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
		<xsl:for-each select="@*">
			<xsl:variable name="att_name" select="name()"/>
			<xsl:variable name="att_value" select="."/>
			<xsl:value-of select="DisplayComps:printValue('Name_', $att_name, $att_value)"/> 
		</xsl:for-each>
    <div class="ui-grid-a">
			<xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
    </div>
    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
		<xsl:for-each select="@*">
			<xsl:variable name="att_name" select="name()"/>
			<xsl:variable name="att_value" select="."/>
			<xsl:value-of select="DisplayComps:printValue('Date_', $att_name, $att_value)"/> 
		</xsl:for-each>
    <div class="ui-grid-a">
			<xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
    </div>
    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
		<xsl:for-each select="@*">
			<xsl:variable name="att_name" select="name()"/>
			<xsl:variable name="att_value" select="."/>
			<xsl:value-of select="DisplayComps:printValue('Num_', $att_name, $att_value)"/> 
		</xsl:for-each>
    <div class="ui-grid-a">
			<xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
    </div>
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
		<xsl:variable name="fullcolcount" select="@Files + 1"/>
    <h4 class="ui-bar-b">
      Outcome Name, Date, Label
    </h4>
		<xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
		<xsl:for-each select="@*">
			<xsl:variable name="att_name" select="name()"/>
			<xsl:variable name="att_value" select="."/>
			<xsl:value-of select="DisplayComps:printValue('Name_', $att_name, $att_value)"/> 
		</xsl:for-each>
    <div class="ui-grid-a">
			<xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
    </div>
    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
		<xsl:for-each select="@*">
			<xsl:variable name="att_name" select="name()"/>
			<xsl:variable name="att_value" select="."/>
			<xsl:value-of select="DisplayComps:printValue('Date_', $att_name, $att_value)"/> 
		</xsl:for-each>
    <div class="ui-grid-a">
			<xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
    </div>
    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
		<xsl:for-each select="@*">
			<xsl:variable name="att_name" select="name()"/>
			<xsl:variable name="att_value" select="."/>
			<xsl:value-of select="DisplayComps:printValue('Num_', $att_name, $att_value)"/> 
		</xsl:for-each>
    <div class="ui-grid-a">
			<xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
    </div>
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
		<xsl:variable name="fullcolcount" select="@Files + 1"/>
    <h4 class="ui-bar-b">
      Operation Name, Date, Label
    </h4>
		<xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
		<xsl:for-each select="@*">
			<xsl:variable name="att_name" select="name()"/>
			<xsl:variable name="att_value" select="."/>
			<xsl:value-of select="DisplayComps:printValue('Name_', $att_name, $att_value)"/> 
		</xsl:for-each>
    <div class="ui-grid-a">
			<xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
    </div>
    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
		<xsl:for-each select="@*">
			<xsl:variable name="att_name" select="name()"/>
			<xsl:variable name="att_value" select="."/>
			<xsl:value-of select="DisplayComps:printValue('Date_', $att_name, $att_value)"/> 
		</xsl:for-each>
    <div class="ui-grid-a">
			<xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
    </div>
    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
		<xsl:for-each select="@*">
			<xsl:variable name="att_name" select="name()"/>
			<xsl:variable name="att_value" select="."/>
			<xsl:value-of select="DisplayComps:printValue('Num_', $att_name, $att_value)"/> 
		</xsl:for-each>
    <div class="ui-grid-a">
			<xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
    </div>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
   <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Nutrient Details
        </h4>
        <h4 class="ui-bar-b">
          Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'Observations_')">
					  <xsl:value-of select="DisplayComps:printValue('Observations_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Target
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TargetType_')">
					  <xsl:value-of select="DisplayComps:printValue('TargetType_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Name
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TMN1Name_')">
					  <xsl:value-of select="DisplayComps:printValue('TMN1Name_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Plan Period 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN1Q_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Plan Full 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN1PFTotal_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Plan Cumul
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN1PCTotal_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Actual Period
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN1APTotal_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Actual Cumul
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN1ACTotal_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Actual Period Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN1APChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Actual Cumul Change 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN1ACChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Plan P Percent 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN1PPPercent_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Plan C Percent 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN1PCPercent_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Plan Full Percent
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN1PFPercent_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Name
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TMN2Name_')">
					  <xsl:value-of select="DisplayComps:printValue('TMN2Name_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Plan Period 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN2Q_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Plan Full 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN2PFTotal_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Plan Cumul
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN2PCTotal_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Actual Period
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN2APTotal_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Actual Cumul
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN2ACTotal_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Actual Period Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN2APChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Actual Cumul Change 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN2ACChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Plan P Percent 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN2PPPercent_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Plan C Percent 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN2PCPercent_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Plan Full Percent
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN2PFPercent_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <xsl:if test="(@FNCount > 2)">
          <h4 class="ui-bar-b">
            Name
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:if test="starts-with($att_name, 'TMN3Name_')">
					    <xsl:value-of select="DisplayComps:printValue('TMN3Name_', $att_name, $att_value)"/> 
				    </xsl:if>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Period 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN3Q_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Full 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN3PFTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Cumul
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN3PCTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Period
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN3APTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Cumul
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN3ACTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Period Change
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN3APChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Cumul Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN3ACChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan P Percent 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN3PPPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan C Percent 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN3PCPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Full Percent
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN3PFPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
       </xsl:if>
       <xsl:if test="(@FNCount > 3)">
          <h4 class="ui-bar-b">
            Name
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:if test="starts-with($att_name, 'TMN4Name_')">
					    <xsl:value-of select="DisplayComps:printValue('TMN4Name_', $att_name, $att_value)"/> 
				    </xsl:if>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Period 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN4Q_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Full 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN4PFTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Cumul
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN4PCTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Period
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN4APTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Cumul
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN4ACTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Period Change
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN4APChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Cumul Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN4ACChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan P Percent 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN4PPPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan C Percent 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN4PCPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Full Percent
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN4PFPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
       </xsl:if>
       <xsl:if test="(@FNCount > 4)">
          <h4 class="ui-bar-b">
            Name
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:if test="starts-with($att_name, 'TMN5Name_')">
					    <xsl:value-of select="DisplayComps:printValue('TMN5Name_', $att_name, $att_value)"/> 
				    </xsl:if>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Period 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN5Q_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Full 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN5PFTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Cumul
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN5PCTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Period
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN5APTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Cumul
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN5ACTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Period Change
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN5APChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Cumul Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN5ACChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan P Percent 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN5PPPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan C Percent 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN5PCPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Full Percent
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN5PFPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
       </xsl:if>
       <xsl:if test="(@FNCount > 5)">
          <h4 class="ui-bar-b">
            Name
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:if test="starts-with($att_name, 'TMN6Name_')">
					    <xsl:value-of select="DisplayComps:printValue('TMN6Name_', $att_name, $att_value)"/> 
				    </xsl:if>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Period 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN6Q_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Full 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN6PFTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Cumul
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN6PCTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Period
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN6APTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Cumul
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN6ACTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Period Change
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN6APChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Cumul Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN6ACChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan P Percent 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN6PPPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan C Percent 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN6PCPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Full Percent
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN6PFPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
       </xsl:if>
       <xsl:if test="(@FNCount > 6)">
          <h4 class="ui-bar-b">
            Name
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:if test="starts-with($att_name, 'TMN7Name_')">
					    <xsl:value-of select="DisplayComps:printValue('TMN7Name_', $att_name, $att_value)"/> 
				    </xsl:if>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Period 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN7Q_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Full 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN7PFTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Cumul
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN7PCTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Period
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN7APTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Cumul
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN7ACTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Period Change
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN7APChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Cumul Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN7ACChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan P Percent 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN7PPPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan C Percent 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN7PCPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Full Percent
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN7PFPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
       </xsl:if>
       <xsl:if test="(@FNCount > 7)">
          <h4 class="ui-bar-b">
            Name
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:if test="starts-with($att_name, 'TMN8Name_')">
					    <xsl:value-of select="DisplayComps:printValue('TMN8Name_', $att_name, $att_value)"/> 
				    </xsl:if>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Period 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN8Q_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Full 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN8PFTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Cumul
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN8PCTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Period
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN8APTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Cumul
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN8ACTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Period Change
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN8APChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Cumul Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN8ACChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan P Percent 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN8PPPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan C Percent 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN8PCPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Full Percent
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN8PFPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
       </xsl:if>
       <xsl:if test="(@FNCount > 8)">
          <h4 class="ui-bar-b">
            Name
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:if test="starts-with($att_name, 'TMN9Name_')">
					    <xsl:value-of select="DisplayComps:printValue('TMN9Name_', $att_name, $att_value)"/> 
				    </xsl:if>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Period 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN9Q_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Full 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN9PFTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Cumul
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN9PCTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Period
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN9APTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Cumul
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN9ACTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Period Change
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN9APChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Cumul Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN9ACChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan P Percent 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN9PPPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan C Percent 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN9PCPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Full Percent
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN9PFPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
       </xsl:if>
       <xsl:if test="(@FNCount > 9)">
          <h4 class="ui-bar-b">
            Name
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:if test="starts-with($att_name, 'TMN10Name_')">
					    <xsl:value-of select="DisplayComps:printValue('TMN10Name_', $att_name, $att_value)"/> 
				    </xsl:if>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Period 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN10Q_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Full 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN10PFTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Cumul
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN10PCTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Period
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN10APTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Cumul
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN10ACTotal_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Period Change
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN10APChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Actual Cumul Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN10ACChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan P Percent 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN10PPPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan C Percent 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN10PCPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Plan Full Percent
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN10PFPercent_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
       </xsl:if>
      </div>
	</xsl:template>
</xsl:stylesheet>

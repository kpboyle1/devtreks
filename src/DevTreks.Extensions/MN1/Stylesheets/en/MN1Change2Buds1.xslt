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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnchangeyr' 
      or @AnalyzerType='mnchangeid' or @AnalyzerType='mnchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budget">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budget">
		<xsl:variable name="fullcolcount" select="@Files + 1"/>
    <h4 class="ui-bar-b">
      Budget Name and Date
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnchangeyr' 
      or @AnalyzerType='mnchangeid' or @AnalyzerType='mnchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgettimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgettimeperiod">
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
    <h4 class="ui-bar-b">
      Time Period Name and Date
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
		<xsl:variable name="fullcolcount" select="@Files + 1"/>
    <h4 class="ui-bar-b">
      Outcome Name and Date
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
		<xsl:variable name="fullcolcount" select="@Files + 1"/>
    <h4 class="ui-bar-b">
      Operation Name and Date
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnchangeyr' 
      or @AnalyzerType='mnchangeid' or @AnalyzerType='mnchangealt']">
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
          Alternative
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'AlternativeType_')">
					  <xsl:value-of select="DisplayComps:printValue('AlternativeType_', $att_name, $att_value)"/> 
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
          Total 
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
          Amount Change 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN1AmountChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Percent Change 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN1PercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Base Change 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN1BaseChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Base Percent Change 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN1BasePercentChange_', $att_name, $att_value)"/>
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
          Total 
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
          Amount Change 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN2AmountChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Percent Change 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN2PercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Base Change 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN2BaseChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Base Percent Change 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN2BasePercentChange_', $att_name, $att_value)"/>
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
            Total 
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
            Amount Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN3AmountChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Percent Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN3PercentChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Base Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN3BaseChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Base Percent Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN3BasePercentChange_', $att_name, $att_value)"/>
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
            Total 
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
            Amount Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN4AmountChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Percent Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN4PercentChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Base Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN4BaseChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Base Percent Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN4BasePercentChange_', $att_name, $att_value)"/>
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
            Total 
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
            Amount Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN5AmountChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Percent Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN5PercentChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Base Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN5BaseChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Base Percent Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN5BasePercentChange_', $att_name, $att_value)"/>
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
            Total 
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
            Amount Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN6AmountChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Percent Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN6PercentChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Base Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN6BaseChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Base Percent Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN6BasePercentChange_', $att_name, $att_value)"/>
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
            Total 
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
            Amount Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN7AmountChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Percent Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN7PercentChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Base Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN7BaseChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Base Percent Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN7BasePercentChange_', $att_name, $att_value)"/>
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
            Total 
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
            Amount Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN8AmountChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Percent Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN8PercentChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Base Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN8BaseChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Base Percent Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN8BasePercentChange_', $att_name, $att_value)"/>
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
            Total 
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
            Amount Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN9AmountChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Percent Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN9PercentChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Base Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN9BaseChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Base Percent Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN9BasePercentChange_', $att_name, $att_value)"/>
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
            Total 
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
            Amount Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN10AmountChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Percent Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN10PercentChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Base Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN10BaseChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Base Percent Change 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN10BasePercentChange_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
     </div>
	</xsl:template>
</xsl:stylesheet>

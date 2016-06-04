<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, June -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Operation"
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
			<xsl:apply-templates select="operationgroup" />
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
		<xsl:apply-templates select="operationgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="operationgroup">
		<h4>
      Operation Group: <xsl:value-of select="@Name" /> ; <xsl:value-of select="@Num_0" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="operation">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="operation">
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
    <h4 class="ui-bar-b">
      Operation Name and Label
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
			<xsl:value-of select="DisplayComps:printValue('Num_', $att_name, $att_value)"/> 
		</xsl:for-each>
    <div class="ui-grid-a">
			<xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
    </div>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Nutrient Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TQN_')">
					  <xsl:value-of select="DisplayComps:printValue('TQN_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
         <h4 class="ui-bar-b">
          Nutrient Name
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
          Mean 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN1Mean_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Median 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN1Median_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Variance 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN1Variance_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Std Dev 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN1StandDev_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
         <h4 class="ui-bar-b">
          Nutrient Name
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
          Mean 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN2Mean_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Median 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN2Median_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Variance 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN2Variance_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Std Dev 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TMN2StandDev_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <xsl:if test="(@FNCount > 2)">
           <h4 class="ui-bar-b">
            Nutrient Name
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
            Mean 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN3Mean_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Median 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN3Median_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Variance 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN3Variance_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Std Dev 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN3StandDev_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@FNCount > 3)">
           <h4 class="ui-bar-b">
            Nutrient Name
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
            Mean 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN4Mean_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Median 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN4Median_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Variance 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN4Variance_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Std Dev 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN4StandDev_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@FNCount > 4)">
          <h4 class="ui-bar-b">
            Nutrient Name
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
            Mean 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN5Mean_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Median 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN5Median_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Variance 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN5Variance_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Std Dev 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN5StandDev_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@FNCount > 5)">
          <h4 class="ui-bar-b">
            Nutrient Name
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
            Mean 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN6Mean_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Median 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN6Median_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Variance 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN6Variance_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Std Dev 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN6StandDev_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@FNCount > 6)">
          <h4 class="ui-bar-b">
            Nutrient Name
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
            Mean 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN7Mean_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Median 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN7Median_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Variance 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN7Variance_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Std Dev 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN7StandDev_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@FNCount > 7)">
          <h4 class="ui-bar-b">
            Nutrient Name
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
            Mean 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN8Mean_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Median 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN8Median_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Variance 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN8Variance_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Std Dev 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN8StandDev_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@FNCount > 8)">
          <h4 class="ui-bar-b">
            Nutrient Name
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
            Mean 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN9Mean_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Median 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN9Median_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Variance 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN9Variance_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Std Dev 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN9StandDev_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@FNCount > 9)">
          <h4 class="ui-bar-b">
            Nutrient Name
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
            Mean 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN10Mean_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Median 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN10Median_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Variance 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN10Variance_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
          <h4 class="ui-bar-b">
            Std Dev 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TMN10StandDev_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
      </div>
	</xsl:template>
</xsl:stylesheet>

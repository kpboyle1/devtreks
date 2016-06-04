<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, December -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Outcome"
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
			<xsl:apply-templates select="outcomegroup" />
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
		<xsl:apply-templates select="outcomegroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcomegroup">
		<h4>
      <strong>Outcome Group</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="outcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcome">
    <h4>
      <strong>Outcome </strong>: <xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" />;&#xA0;Label:&#xA0;<xsl:value-of select="@Num" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="outcomeoutput">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcomeoutput">
    <h4>
      <strong>Output </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:if test="($localName != 'outcomeoutput')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Outcome Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Date : <xsl:value-of select="@Date"/>
          </div>
          <div class="ui-block-b">
            Observations : <xsl:value-of select="@Observations"/>; Target Type : <xsl:value-of select="@TargetType"/>
          </div>
          <div class="ui-block-a">
            R Planned Period : <xsl:value-of select="@TAMR"/>
          </div>
          <div class="ui-block-b">
            R Plan Full : <xsl:value-of select="@TAMRPFTotal"/>
          </div>
          <div class="ui-block-a">
            R Plan Cumul : <xsl:value-of select="@TAMRPCTotal"/>
          </div>
          <div class="ui-block-b">
            R Actual Period : <xsl:value-of select="@TAMRAPTotal"/>
          </div>
          <div class="ui-block-a">
            R Actual Cumul : <xsl:value-of select="@TAMRACTotal"/>
          </div>
          <div class="ui-block-b">
            R Actual Period Change : <xsl:value-of select="@TAMRAPChange"/>
          </div>
          <div class="ui-block-a">
            R Actual Cumul Change : <xsl:value-of select="@TAMRACChange"/>
          </div>
          <div class="ui-block-b">
            R Planned Period Percent : <xsl:value-of select="@TAMRPPPercent"/>
          </div>
          <div class="ui-block-a">
            R Planned Cumul Percent : <xsl:value-of select="@TAMRPCPercent"/>
          </div>
          <div class="ui-block-b">
            R Planned Full Percent : <xsl:value-of select="@TAMRPFPercent"/>
          </div>
          <div class="ui-block-a">
            OutputQ Planned Period : <xsl:value-of select="@TRAmount"/>
          </div>
          <div class="ui-block-b">
            OutputQ Plan Full : <xsl:value-of select="@TRAmountPFTotal"/>
          </div>
          <div class="ui-block-a">
            OutputQ Plan Cumul : <xsl:value-of select="@TRAmountPCTotal"/>
          </div>
          <div class="ui-block-b">
            OutputQ Actual Period : <xsl:value-of select="@TRAmountAPTotal"/>
          </div>
          <div class="ui-block-a">
            OutputQ Actual Cumul : <xsl:value-of select="@TRAmountACTotal"/>
          </div>
          <div class="ui-block-b">
            OutputQ Actual Period Change : <xsl:value-of select="@TRAmountAPChange"/>
          </div>
          <div class="ui-block-a">
            OutputQ Actual Cumul Change : <xsl:value-of select="@TRAmountACChange"/>
          </div>
          <div class="ui-block-b">
            OutputQ Planned Period Percent : <xsl:value-of select="@TRAmountPPPercent"/>
          </div>
          <div class="ui-block-a">
            OutputQ Planned Cumul Percent : <xsl:value-of select="@TRAmountPCPercent"/>
          </div>
          <div class="ui-block-b">
            OutputQ Planned Full Percent : <xsl:value-of select="@TRAmountPFPercent"/>
          </div>
          <div class="ui-block-a">
            RIncent Planned Period : <xsl:value-of select="@TAMRINCENT"/>
          </div>
          <div class="ui-block-b">
            RIncent Plan Full : <xsl:value-of select="@TAMRIncentPFTotal"/>
          </div>
          <div class="ui-block-a">
            RIncent Plan Cumul : <xsl:value-of select="@TAMRIncentPCTotal"/>
          </div>
          <div class="ui-block-b">
            RIncent Actual Period : <xsl:value-of select="@TAMRIncentAPTotal"/>
          </div>
          <div class="ui-block-a">
            RIncent Actual Cumul : <xsl:value-of select="@TAMRIncentACTotal"/>
          </div>
          <div class="ui-block-b">
            RIncent Actual Period Change : <xsl:value-of select="@TAMRIncentAPChange"/>
          </div>
          <div class="ui-block-a">
            RIncent Actual Cumul Change : <xsl:value-of select="@TAMRIncentACChange"/>
          </div>
          <div class="ui-block-b">
            RIncent Planned Period Percent : <xsl:value-of select="@TAMRIncentPPPercent"/>
          </div>
          <div class="ui-block-a">
            RIncent Planned Cumul Percent : <xsl:value-of select="@TAMRIncentPCPercent"/>
          </div>
          <div class="ui-block-b">
            RIncent Planned Full Percent : <xsl:value-of select="@TAMRIncentPFPercent"/>
          </div>
          <div class="ui-block-a">
            RPrice Planned Period : <xsl:value-of select="@TRPrice"/>
          </div>
          <div class="ui-block-b">
            RPrice Plan Full : <xsl:value-of select="@TRPricePFTotal"/>
          </div>
          <div class="ui-block-a">
            RPrice Plan Cumul : <xsl:value-of select="@TRPricePCTotal"/>
          </div>
          <div class="ui-block-b">
            RPrice Actual Period : <xsl:value-of select="@TRPriceAPTotal"/>
          </div>
          <div class="ui-block-a">
            RPrice Actual Cumul : <xsl:value-of select="@TRPriceACTotal"/>
          </div>
          <div class="ui-block-b">
            RPrice Actual Period Change : <xsl:value-of select="@TRPriceAPChange"/>
          </div>
          <div class="ui-block-a">
            RPrice Actual Cumul Change : <xsl:value-of select="@TRPriceACChange"/>
          </div>
          <div class="ui-block-b">
            RPrice Planned Period Percent : <xsl:value-of select="@TRPricePPPercent"/>
          </div>
          <div class="ui-block-a">
            RPrice Planned Cumul Percent : <xsl:value-of select="@TRPricePCPercent"/>
          </div>
          <div class="ui-block-b">
            RPrice Planned Full Percent : <xsl:value-of select="@TRPricePFPercent"/>
          </div>
        </div>
      </div>
		</xsl:if>
		<xsl:if test="($localName = 'outcomeoutput')">
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Output Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Total Revenue : <xsl:value-of select="@TAMR"/>
          </div>
          <div class="ui-block-b">
            Total OutputQ : <xsl:value-of select="@TRAmount"/>
          </div>
          <div class="ui-block-a">
            Total Incent : <xsl:value-of select="@TAMRINCENT"/>
          </div>
          <div class="ui-block-b">
            Total RPrice : <xsl:value-of select="@TRPrice"/>
          </div>
        </div>
      </div>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>

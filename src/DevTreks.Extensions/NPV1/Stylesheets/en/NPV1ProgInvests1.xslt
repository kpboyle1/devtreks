<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, December -->
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
      <strong>Investment Group</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investment">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investment">
		<h4>
      <strong>Investment</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investmenttimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmenttimeperiod">
		<h4>
      <strong>Time Period</strong> : <xsl:value-of select="@Name" />Label:&#xA0;<xsl:value-of select="@Num" />
    </h4>
    <xsl:apply-templates select="root/linkedview[@AnalyzerType='npvprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <div>
      <strong>Outcomes</strong>
    </div>
		<xsl:apply-templates select="investmentoutcomes" />
    <div>
      <strong>Components</strong>
    </div>
		<xsl:apply-templates select="investmentcomponents" />
	</xsl:template>
	<xsl:template match="investmentoutcomes">
		<xsl:apply-templates select="investmentoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutcome">
		<h4>
      <strong>Outcome </strong>: <xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" />;&#xA0;Label:&#xA0;<xsl:value-of select="@Num" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investmentoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutput">
    <h4>
      <strong>Output </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvprogress1']">
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
      <strong>Component</strong>: <xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" />;&#xA0;Label:&#xA0;<xsl:value-of select="@Num" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investmentinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentinput">
		<h4>
      <strong>Input </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName != 'investmentinput' and $localName != 'investmentoutput')">
      <xsl:if test="($localName != 'investmentcomponent')">
        <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
          <h4 class="ui-bar-b">
            <strong>Benefit Details</strong>
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
      <xsl:if test="($localName != 'investmentoutcome')">
        <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Cost Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Date : <xsl:value-of select="@Date"/>
          </div>
          <div class="ui-block-b">
            Observations : <xsl:value-of select="@Observations"/>; Target Type : <xsl:value-of select="@TargetType"/>
          </div>
          <div class="ui-block-a">
            OC Planned Period : <xsl:value-of select="@TAMOC"/>
          </div>
          <div class="ui-block-b">
            OC Plan Full : <xsl:value-of select="@TAMOCPFTotal"/>
          </div>
          <div class="ui-block-a">
            OC Plan Cumul : <xsl:value-of select="@TAMOCPCTotal"/>
          </div>
          <div class="ui-block-b">
            OC Actual Period : <xsl:value-of select="@TAMOCAPTotal"/>
          </div>
          <div class="ui-block-a">
            OC Actual Cumul : <xsl:value-of select="@TAMOCACTotal"/>
          </div>
          <div class="ui-block-b">
            OC Actual Period Change : <xsl:value-of select="@TAMOCAPChange"/>
          </div>
          <div class="ui-block-a">
            OC Actual Cumul Change : <xsl:value-of select="@TAMOCACChange"/>
          </div>
          <div class="ui-block-b">
            OC Planned Period Percent : <xsl:value-of select="@TAMOCPPPercent"/>
          </div>
          <div class="ui-block-a">
            OC Planned Cumul Percent : <xsl:value-of select="@TAMOCPCPercent"/>
          </div>
          <div class="ui-block-b">
            OC Planned Full Percent : <xsl:value-of select="@TAMOCPFPercent"/>
          </div>
          <div class="ui-block-a">
            AOH Planned Period : <xsl:value-of select="@TAMAOH"/>
          </div>
          <div class="ui-block-b">
            AOH Plan Full : <xsl:value-of select="@TAMAOHPFTotal"/>
          </div>
          <div class="ui-block-a">
            AOH Plan Cumul : <xsl:value-of select="@TAMAOHPCTotal"/>
          </div>
          <div class="ui-block-b">
            AOH Actual Period : <xsl:value-of select="@TAMAOHAPTotal"/>
          </div>
          <div class="ui-block-a">
            AOH Actual Cumul : <xsl:value-of select="@TAMAOHACTotal"/>
          </div>
          <div class="ui-block-b">
            AOH Actual Period Change : <xsl:value-of select="@TAMAOHAPChange"/>
          </div>
          <div class="ui-block-a">
            AOH Actual Cumul Change : <xsl:value-of select="@TAMAOHACChange"/>
          </div>
          <div class="ui-block-b">
            AOH Planned Period Percent : <xsl:value-of select="@TAMAOHPPPercent"/>
          </div>
          <div class="ui-block-a">
            AOH Planned Cumul Percent : <xsl:value-of select="@TAMAOHPCPercent"/>
          </div>
          <div class="ui-block-b">
            AOH Planned Full Percent : <xsl:value-of select="@TAMAOHPFPercent"/>
          </div>
          <div class="ui-block-a">
            CAP Planned Period : <xsl:value-of select="@TAMCAP"/>
          </div>
          <div class="ui-block-b">
            CAP Plan Full : <xsl:value-of select="@TAMCAPPFTotal"/>
          </div>
          <div class="ui-block-a">
            CAP Plan Cumul : <xsl:value-of select="@TAMCAPPCTotal"/>
          </div>
          <div class="ui-block-b">
            CAP Actual Period : <xsl:value-of select="@TAMCAPAPTotal"/>
          </div>
          <div class="ui-block-a">
            CAP Actual Cumul : <xsl:value-of select="@TAMCAPACTotal"/>
          </div>
          <div class="ui-block-b">
            CAP Actual Period Change : <xsl:value-of select="@TAMCAPAPChange"/>
          </div>
          <div class="ui-block-a">
            CAP Actual Cumul Change : <xsl:value-of select="@TAMCAPACChange"/>
          </div>
          <div class="ui-block-b">
            CAP Planned Period Percent : <xsl:value-of select="@TAMCAPPPPercent"/>
          </div>
          <div class="ui-block-a">
            CAP Planned Cumul Percent : <xsl:value-of select="@TAMCAPPCPercent"/>
          </div>
          <div class="ui-block-b">
            CAP Planned Full Percent : <xsl:value-of select="@TAMCAPPFPercent"/>
          </div>
          <div class="ui-block-a">
            Total Planned Period : <xsl:value-of select="@TAMTOTAL"/>
          </div>
          <div class="ui-block-b">
            Total Plan Full : <xsl:value-of select="@TAMPFTotal"/>
          </div>
          <div class="ui-block-a">
            Total Plan Cumul : <xsl:value-of select="@TAMPCTotal"/>
          </div>
          <div class="ui-block-b">
            Total Actual Period : <xsl:value-of select="@TAMAPTotal"/>
          </div>
          <div class="ui-block-a">
            Total Actual Cumul : <xsl:value-of select="@TAMACTotal"/>
          </div>
          <div class="ui-block-b">
            Total Actual Period Change : <xsl:value-of select="@TAMAPChange"/>
          </div>
          <div class="ui-block-a">
            Total Actual Cumul Change : <xsl:value-of select="@TAMACChange"/>
          </div>
          <div class="ui-block-b">
            Total Planned Period Percent : <xsl:value-of select="@TAMPPPercent"/>
          </div>
          <div class="ui-block-a">
            Total Planned Cumul Percent : <xsl:value-of select="@TAMPCPercent"/>
          </div>
          <div class="ui-block-b">
            Total Planned Full Percent : <xsl:value-of select="@TAMPFPercent"/>
          </div>
          <div class="ui-block-a">
            Incent Planned Period : <xsl:value-of select="@TAMINCENT"/>
          </div>
          <div class="ui-block-b">
            Incent Plan Full : <xsl:value-of select="@TAMIncentPFTotal"/>
          </div>
          <div class="ui-block-a">
            Incent Plan Cumul : <xsl:value-of select="@TAMIncentPCTotal"/>
          </div>
          <div class="ui-block-b">
            Incent Actual Period : <xsl:value-of select="@TAMIncentAPTotal"/>
          </div>
          <div class="ui-block-a">
            Incent Actual Cumul : <xsl:value-of select="@TAMIncentACTotal"/>
          </div>
          <div class="ui-block-b">
            Incent Actual Period Change : <xsl:value-of select="@TAMIncentAPChange"/>
          </div>
          <div class="ui-block-a">
            Incent Actual Cumul Change : <xsl:value-of select="@TAMIncentACChange"/>
          </div>
          <div class="ui-block-b">
            Incent Planned Period Percent : <xsl:value-of select="@TAMIncentPPPercent"/>
          </div>
          <div class="ui-block-a">
            Incent Planned Cumul Percent : <xsl:value-of select="@TAMIncentPCPercent"/>
          </div>
          <div class="ui-block-b">
            Incent Planned Full Percent : <xsl:value-of select="@TAMIncentPFPercent"/>
          </div>
          <div class="ui-block-a">
            Net Planned Period : <xsl:value-of select="@TAMNET"/>
          </div>
          <div class="ui-block-b">
            Net Plan Full : <xsl:value-of select="@TAMNETPFTotal"/>
          </div>
          <div class="ui-block-a">
            Net Plan Cumul : <xsl:value-of select="@TAMNETPCTotal"/>
          </div>
          <div class="ui-block-b">
            Net Actual Period : <xsl:value-of select="@TAMNETAPTotal"/>
          </div>
          <div class="ui-block-a">
            Net Actual Cumul : <xsl:value-of select="@TAMNETACTotal"/>
          </div>
          <div class="ui-block-b">
            Net Actual Period Change : <xsl:value-of select="@TAMNETAPChange"/>
          </div>
          <div class="ui-block-a">
            Net Actual Cumul Change : <xsl:value-of select="@TAMNETACChange"/>
          </div>
          <div class="ui-block-b">
            Net Planned Period Percent : <xsl:value-of select="@TAMNETPPPercent"/>
          </div>
          <div class="ui-block-a">
            Net Planned Cumul Percent : <xsl:value-of select="@TAMNETPCPercent"/>
          </div>
          <div class="ui-block-b">
            Net Planned Full Percent : <xsl:value-of select="@TAMNETPFPercent"/>
          </div>
        </div>
      </div>
      </xsl:if>
		</xsl:if>
		<xsl:if test="($localName = 'investmentinput')">
			<div data-role="collapsible"  data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Input Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
           Total OC : <xsl:value-of select="@TAMOC"/>
          </div>
          <div class="ui-block-b">
            Total AOH : <xsl:value-of select="@TAMAOH"/>
          </div>
          <div class="ui-block-a">
            Total CAP : <xsl:value-of select="@TAMCAP"/>
          </div>
          <div class="ui-block-b">
           Total Total : <xsl:value-of select="@TAMTOTAL"/>
          </div>
          <div class="ui-block-b">
            Total Net : <xsl:value-of select="@TAMNET"/>
          </div>
          <div class="ui-block-a">
            Total Incent : <xsl:value-of select="@TAMINCENT"/>
          </div>
          <div class="ui-block-b">
          </div>
        </div>
      </div>
		</xsl:if>
    <xsl:if test="($localName = 'investmentoutput')">
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

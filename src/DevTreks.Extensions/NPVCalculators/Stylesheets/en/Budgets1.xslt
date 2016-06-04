<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, December -->
<xsl:stylesheet version="1.0" 
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:devtreks="http://devtreks.org"
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
	<!-- which node to start with?  -->
	<xsl:param name="nodeName" />
	<!-- which view to use? -->
	<xsl:param name="viewEditType" />
	<!-- is this a coordinator? -->
	<xsl:param name="memberRole" />
	<!-- what is the current uri? -->
	<xsl:param name="selectedFileURIPattern" />
	<!-- the addin being used -->
	<xsl:param name="calcDocURI" />
	<!-- the node being calculated (in the case of customdocs this will be a DEVDOCS_TYPES) -->
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
	<!-- what is the owning club's email? -->
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
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
    <h4>
			Service: <xsl:value-of select="@Name" />
		</h4>
		<xsl:apply-templates select="budgetgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetgroup">
			<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
			<h4>
        <strong>Budget Group</strong> : <xsl:value-of select="@Name" />
      </h4>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b">
          <strong>Budget Group Details</strong>
        </h4>
			  <div>
					  Document Status : <xsl:value-of select="@DocStatus" />
			  </div>
        <div >
				  Description : <xsl:value-of select="@Description" />
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
				    Type: : 
					  <xsl:value-of select="DisplayDevPacks:WriteSelectListForTypes($searchurl, @ServiceId, @TypeId, 'TypeId', $viewEditType)"/>
				  </div>
          <div class="ui-block-b">
					  Label : <xsl:value-of select="@Num" />
        </div>
        <div class="ui-block-a">
				    Date : <xsl:value-of select="@Date" />
				  </div>
          <div class="ui-block-b">
					  Last Changed : <xsl:value-of select="@LastChangedDate" />
        </div>
      </div>
      <xsl:if test="(@TAMNET != 'NaN')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Total Ben : <xsl:value-of select="@TR"/>
          </div>
          <div class="ui-block-b">
            Ann Ben : <xsl:value-of select="@TAMR"/>
          </div>
          <div class="ui-block-a">
            Total OC Cost : <xsl:value-of select="@TOC"/>
          </div>
          <div class="ui-block-b">
            Ann OC Cost : <xsl:value-of select="@TAMOC"/>
          </div>
          <div class="ui-block-a">
            Net OC Profits : <xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TOC, 0, 0)"/>
          </div>
          <div class="ui-block-b">
            Ann Net OC Profits : <xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TAMR, @TAMOC, 0, 0)"/>
          </div>
          <div class="ui-block-a">
            Total AOH Cost : <xsl:value-of select="@TAOH"/>
          </div>
          <div class="ui-block-b">
            Ann AOH Cost : <xsl:value-of select="@TAMAOH"/>
          </div>
          <div class="ui-block-a">
            Net AOH Profits : <xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TOC, @TAOH, 0)"/>
          </div>
          <div class="ui-block-b">
            Ann Net AOH Profits : <xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TAMR, @TAMOC, @TAMAOH, 0)"/>
          </div>
          <div class="ui-block-a">
            Total CAP Cost : <xsl:value-of select="@TCAP"/>
          </div>
          <div class="ui-block-b">
            Ann CAP Cost : <xsl:value-of select="@TAMCAP"/>
          </div>
          <div class="ui-block-a">
            Net Profits : <xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TOC, @TAOH, @TCAP)"/>
          </div>
          <div class="ui-block-b">
            Ann Net Profits : <xsl:value-of select="@TAMNET"/>
          </div>
          <div class="ui-block-a">
            Incent Ben : <xsl:value-of select="@TRINCENT"/>
          </div>
          <div class="ui-block-b">
            Ann Incent Ben : <xsl:value-of select="@TAMRINCENT"/>
          </div>
          <div class="ui-block-a">
            Incent Cost : <xsl:value-of select="@TINCENT"/>
          </div>
          <div class="ui-block-b">
            Ann Incent Cost : <xsl:value-of select="@TAMINCENT"/>
          </div>
          <div class="ui-block-a">
            Net Incent Cost : <xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TRINCENT, @TINCENT, 0, 0)"/>
          </div>
          <div class="ui-block-b">
            Net Ann Incent Profit : <xsl:value-of select="@TAMINCENT_NET"/>
          </div>
      </div>
      </xsl:if>
    </div>
		<xsl:apply-templates select="budget">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budget">
      <h4>
        <strong>Budget : </strong><xsl:value-of select="@Name" />
      </h4>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b">
          <strong>Budget Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Last Changed : <xsl:value-of select="@LastChangedDate" />
          </div>
          <div class="ui-block-b">
            Ending Date : <xsl:value-of select="@Date" />
          </div>
          <div class="ui-block-a">
            Initial Value : <xsl:value-of select="@InitialValue" />
          </div>
          <div class="ui-block-b">
            Salvage Value : <xsl:value-of select="@SalvageValue" />
          </div>
          <div class="ui-block-a">
            Label : <xsl:value-of select="@Num" />
          </div>
          <div class="ui-block-b">
            Label 2 : <xsl:value-of select="@Num2" />
          </div>
        </div>
		  <div >
			  Description : <xsl:value-of select="@Description" />
	    </div>
    </div>
    <xsl:if test="(@TAMNET != 'NaN')">
      <div class="ui-grid-a">
        <div class="ui-block-a">
         Total Ben : <xsl:value-of select="@TR"/>
        </div>
        <div class="ui-block-b">
           Ann Ben : <xsl:value-of select="@TAMR"/>
        </div>
        <div class="ui-block-a">
          Total OC Cost : <xsl:value-of select="@TOC"/>
        </div>
        <div class="ui-block-b">
          Ann OC Cost : <xsl:value-of select="@TAMOC"/>
        </div>
        <div class="ui-block-a">
          Net OC Profits : <xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TOC, 0, 0)"/>
        </div>
        <div class="ui-block-b">
          Ann Net OC Profits : <xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TAMR, @TAMOC, 0, 0)"/>
        </div>
        <div class="ui-block-a">
          Total AOH Cost : <xsl:value-of select="@TAOH"/>
        </div>
        <div class="ui-block-b">
          Ann AOH Cost : <xsl:value-of select="@TAMAOH"/>
        </div>
        <div class="ui-block-a">
          Net AOH Profits : <xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TOC, @TAOH, 0)"/>
        </div>
        <div class="ui-block-b">
          Ann Net AOH Profits : <xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TAMR, @TAMOC, @TAMAOH, 0)"/>
        </div>
        <div class="ui-block-a">
          Total CAP Cost : <xsl:value-of select="@TCAP"/>
        </div>
        <div class="ui-block-b">
          Ann CAP Cost : <xsl:value-of select="@TAMCAP"/>
        </div>
        <div class="ui-block-a">
          Net Profits : <xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TOC, @TAOH, @TCAP)"/>
        </div>
        <div class="ui-block-b">
          Ann Net Profits : <xsl:value-of select="@TAMNET"/>
        </div>
        <div class="ui-block-a">
            Incent Ben : <xsl:value-of select="@TRINCENT"/>
          </div>
          <div class="ui-block-b">
            Ann Incent Ben : <xsl:value-of select="@TAMRINCENT"/>
          </div>
          <div class="ui-block-a">
            Incent Cost : <xsl:value-of select="@TINCENT"/>
          </div>
          <div class="ui-block-b">
            Ann Incent Cost : <xsl:value-of select="@TAMINCENT"/>
          </div>
          <div class="ui-block-a">
            Net Incent Cost : <xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TRINCENT, @TINCENT, 0, 0)"/>
          </div>
          <div class="ui-block-b">
            Net Ann Incent Profit : <xsl:value-of select="@TAMINCENT_NET"/>
          </div>
          <div class="ui-block-a">
            Equiv Ann Ann : <xsl:value-of select="@InvestmentEAA"/>
          </div>
          <div class="ui-block-b">
            
          </div>
      </div>
    </xsl:if>
		<xsl:apply-templates select="budgettimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgettimeperiod">
    <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
		  <h4 class="ui-bar-b">
        <strong>Time Period : <xsl:value-of select="@EnterpriseName" /></strong>
      </h4>
      <div>
        <strong>Name</strong> : <xsl:value-of select="@Name" />
      </div>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b">
          <strong>Time Period Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Last Changed : <xsl:value-of select="@LastChangedDate" />
          </div>
          <div class="ui-block-b">
            Ending Date : <xsl:value-of select="@Date" />
          </div>
          <div class="ui-block-a">
            Amount : <xsl:value-of select="@EnterpriseAmount" />
          </div>
          <div class="ui-block-b">
            Common Ref? : <xsl:value-of select="@CommonRefYorNTrue" />
          </div>
          <div class="ui-block-a">
            Discount? : <xsl:value-of select="@DiscountYorNTrue" />
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@EnterpriseUnit" />
          </div>
          <div class="ui-block-a">
            IncentAmount : <xsl:value-of select="@IncentiveAmount" />
          </div>
          <div class="ui-block-b">
            IncentRate : <xsl:value-of select="@IncentiveRate" />
          </div>
          <div class="ui-block-a">
            GrowthType : <xsl:value-of select="@GrowthTypeId" />
          </div>
          <div class="ui-block-b">
            Growth Periods : <xsl:value-of select="@GrowthPeriods" />
          </div>
          <div class="ui-block-a">
            Label : <xsl:value-of select="@Num" />
          </div>
          <div class="ui-block-b">
            AOH Factor : <xsl:value-of select="@AOHFactor" />
          </div>
        </div>
		  <div >
			  Description : <xsl:value-of select="@Description" />
	    </div>
    </div>
    <div class="ui-grid-a">
      <div class="ui-block-a">
        Total Ben : <xsl:value-of select="@TR"/>
      </div>
      <div class="ui-block-b">
        Ann Ben : <xsl:value-of select="@TAMR"/>
      </div>
      <div class="ui-block-a">
        Total OC Cost : <xsl:value-of select="@TOC"/>
      </div>
      <div class="ui-block-b">
        Ann OC Cost : <xsl:value-of select="@TAMOC"/>
      </div>
      <div class="ui-block-a">
        Net OC Profits : <xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TOC, 0, 0)"/>
      </div>
      <div class="ui-block-b">
        Ann Net OC Profits : <xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TAMR, @TAMOC, 0, 0)"/>
      </div>
      <div class="ui-block-a">
        Total AOH Cost : <xsl:value-of select="@TAOH"/>
      </div>
      <div class="ui-block-b">
        Ann AOH Cost : <xsl:value-of select="@TAMAOH"/>
      </div>
      <div class="ui-block-a">
        Net AOH Profits : <xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TOC, @TAOH, 0)"/>
      </div>
      <div class="ui-block-b">
        Ann Net AOH Profits : <xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TAMR, @TAMOC, @TAMAOH, 0)"/>
      </div>
      <div class="ui-block-a">
        Total CAP Cost : <xsl:value-of select="@TCAP"/>
      </div>
      <div class="ui-block-b">
        Ann CAP Cost : <xsl:value-of select="@TAMCAP"/>
      </div>
      <div class="ui-block-a">
        Net Profits : <xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TOC, @TAOH, @TCAP)"/>
      </div>
      <div class="ui-block-b">
        Ann Net Profits : <xsl:value-of select="@TAMNET"/>
      </div>
      <div class="ui-block-a">
        Incent Ben : <xsl:value-of select="@TRINCENT"/>
      </div>
      <div class="ui-block-b">
        Ann Incent Ben : <xsl:value-of select="@TAMRINCENT"/>
      </div>
      <div class="ui-block-a">
        Incent Cost : <xsl:value-of select="@TINCENT"/>
      </div>
      <div class="ui-block-b">
        Ann Incent Cost : <xsl:value-of select="@TAMINCENT"/>
      </div>
      <div class="ui-block-a">
        Net Incent Cost : <xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TRINCENT, @TINCENT, 0, 0)"/>
      </div>
      <div class="ui-block-b">
        Net Ann Incent Profit : <xsl:value-of select="@TAMINCENT_NET"/>
      </div>
    </div>
    <!-- jquery can't handle very large html docs -->
    <xsl:if test="($docToCalcNodeName != 'budgetgroup')">
		  <h4 class="ui-bar-b">
        Revenues
		  </h4>
		  <xsl:apply-templates select="budgetoutcomes" />
		  <h4 class="ui-bar-b">
        Costs
      </h4>
		  <xsl:apply-templates select="budgetoperations" />
    </xsl:if>
  </div>
	</xsl:template>
  <xsl:template match="budgetoutcomes">
		<xsl:apply-templates select="budgetoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutcome">
		<h4>
        <strong>Outcome</strong> : <xsl:value-of select="@Name" />
      </h4>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b">
          <strong>Outcome Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Last Changed : <xsl:value-of select="@LastChangedDate" />
          </div>
          <div class="ui-block-b">
            Date : <xsl:value-of select="@Date" />
          </div>
          <div class="ui-block-a">
            Amount : <xsl:value-of select="@Amount" />
          </div>
          <div class="ui-block-b">
            Life : <xsl:value-of select="@EffectiveLife" />
          </div>
          <div class="ui-block-a">
            Salvage : <xsl:value-of select="@SalvageValue" />
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@Unit" />
          </div>
          <div class="ui-block-a">
           IncentAmount : <xsl:value-of select="@IncentiveAmount" />
          </div>
          <div class="ui-block-b">
            IncentRate : <xsl:value-of select="@IncentiveRate" />
          </div>
          <div class="ui-block-a">
            Rates (R and N) : <xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('RealRate', @RealRate, 'double', '8')"/>&#xA0;&#xA0;<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('NominalRate', @NominalRate, 'double', '8')"/>
          </div>
          <div class="ui-block-b">
          </div>
        </div>
		  <div >
			  Description : <xsl:value-of select="@Description" />
	    </div>
    </div>
    <div class="ui-grid-a">
      <div class="ui-block-a">
        Total Ben : <xsl:value-of select="@TR"/>
      </div>
      <div class="ui-block-b">
         Ann Ben : <xsl:value-of select="@TAMR"/>
      </div>
      <div class="ui-block-a">
        Total Ben Int : <xsl:value-of select="@TR_INT"/>
      </div>
      <div class="ui-block-b">
        Ann Ben Int : <xsl:value-of select="@TAMR_INT"/>
      </div>
      <div class="ui-block-a">
        Incent Ben : <xsl:value-of select="@TRINCENT"/>
      </div>
      <div class="ui-block-b">
        Ann Incent Ben : <xsl:value-of select="@TAMRINCENT"/>
      </div>
    </div>
    <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
		  <h4 class="ui-bar-b">
        <strong>Outputs</strong>
      </h4>
      <xsl:apply-templates select="budgetoutput">
        <xsl:sort select="@OutputDate"/>
      </xsl:apply-templates>
    </div>
	</xsl:template>
	<xsl:template match="budgetoutput">
		<h4>
      <strong>Output</strong> : <xsl:value-of select="@Name" />
    </h4>
    <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b">
          <strong>Output Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
              Date : <xsl:value-of select="@OutputDate" />
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Times : <xsl:value-of select="@OutputTimes" />
          </div>
          <div class="ui-block-b">
            Comp Amount : <xsl:value-of select="@OutputCompositionAmount" />
          </div>
          <div class="ui-block-a">
            Comp Unit : <xsl:value-of select="@OutputCompositionUnit" />
          </div>
          <div class="ui-block-b">
            Amount : <xsl:value-of select="@OutputAmount1" />
          </div>
          <div class="ui-block-a">
            Unit : <xsl:value-of select="@OutputUnit1" />
          </div>
          <div class="ui-block-b">
            Price : <xsl:value-of select="@OutputPrice1" />
          </div>
          <div class="ui-block-a">
            Incent Amount : <xsl:value-of select="@IncentiveAmount" />
          </div>
          <div class="ui-block-b">
            Incent Rate : <xsl:value-of select="@IncentiveRate" />
          </div>
        </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Ben : <xsl:value-of select="@TR"/>
          </div>
          <div class="ui-block-b">
            Total Ben Int : <xsl:value-of select="@TR_INT"/>
          </div>
          <div class="ui-block-a">
            Total Ben : <xsl:value-of select="DisplayDevPacks:GetAddNumberN2(@TR, @TR_INT, 0, 0)"/>
          </div>
          <div class="ui-block-b">
            Incent Ben : <xsl:value-of select="@TRINCENT"/>
          </div>
        </div>
        <div >
			    Description : <xsl:value-of select="@Description" />
	      </div>
      </div>
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
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b">
          <strong>Operation Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Last Changed : <xsl:value-of select="@LastChangedDate" />
          </div>
          <div class="ui-block-b">
            Date : <xsl:value-of select="@Date" />
          </div>
          <div class="ui-block-a">
            Amount : <xsl:value-of select="@Amount" />
          </div>
          <div class="ui-block-b">
            Life : <xsl:value-of select="@EffectiveLife" />
          </div>
          <div class="ui-block-a">
            Salvage : <xsl:value-of select="@SalvageValue" />
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@Unit" />
          </div>
          <div class="ui-block-a">
           IncentAmount : <xsl:value-of select="@IncentiveAmount" />
          </div>
          <div class="ui-block-b">
            IncentRate : <xsl:value-of select="@IncentiveRate" />
          </div>
        </div>
		  <div >
			  Description : <xsl:value-of select="@Description" />
	    </div>
    </div>
    <div class="ui-grid-a">
      <div class="ui-block-a">
        Total OC Cost : <xsl:value-of select="@TOC"/>
      </div>
      <div class="ui-block-b">
         Ann OC Cost : <xsl:value-of select="@TAMOC"/>
      </div>
      <div class="ui-block-a">
        Total OC Int : <xsl:value-of select="@TOC_INT"/>
      </div>
      <div class="ui-block-b">
      </div>
      <div class="ui-block-a">
        Total AOH Cost : <xsl:value-of select="@TAOH"/>
      </div>
      <div class="ui-block-b">
         Ann AOH Cost : <xsl:value-of select="@TAMAOH"/>
      </div>
      <div class="ui-block-a">
        Total AOH Int : <xsl:value-of select="@TAOH_INT"/>
      </div>
      <div class="ui-block-b">
      </div>
      <div class="ui-block-a">
        Total CAP Cost : <xsl:value-of select="@TCAP"/>
      </div>
      <div class="ui-block-b">
         Ann CAP Cost : <xsl:value-of select="@TAMCAP"/>
      </div>
      <div class="ui-block-a">
        Total CAP Int : <xsl:value-of select="@TCAP_INT"/>
      </div>
      <div class="ui-block-b">
      </div>
      <div class="ui-block-a">
        Total Cost : <xsl:value-of select="DisplayDevPacks:GetAddNumberN2(@TOC, @TAOH, @TCAP, 0)"/>
      </div>
      <div class="ui-block-b">
         Ann Cost : <xsl:value-of select="DisplayDevPacks:GetAddNumberN2(@TAMOC, @TAMAOH, @TAMCAP, 0)"/>
      </div>
      <div class="ui-block-a">
        Total Int : <xsl:value-of select="DisplayDevPacks:GetAddNumberN2(@TOC_INT, @TAOH_INT, @TCAP_INT, 0)"/>
      </div>
      <div class="ui-block-b">
      </div>
      <div class="ui-block-a">
        Incent Cost : <xsl:value-of select="@TINCENT"/>
      </div>
      <div class="ui-block-b">
        Ann Incent Cost : <xsl:value-of select="@TAMINCENT"/>
      </div>
    </div>
    <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
		  <h4 class="ui-bar-b">
        <strong>Inputs</strong>
      </h4>
      <xsl:apply-templates select="budgetinput">
        <xsl:sort select="@InputDate"/>
      </xsl:apply-templates>
    </div>
	</xsl:template>
	<xsl:template match="budgetinput">
		<h4>
      <strong>Input</strong> : <xsl:value-of select="@Name" />
    </h4>
    <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
      <h4 class="ui-bar-b">
        <strong>Input Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Date : <xsl:value-of select="@InputDate" />
        </div>
        <div class="ui-block-b">
          Times : <xsl:value-of select="@InputTimes" />
        </div>
        <div class="ui-block-a">
          IncentAmount : <xsl:value-of select="@IncentiveAmount" />
        </div>
        <div class="ui-block-b">
          IncentRate : <xsl:value-of select="@IncentiveRate" />
        </div>
        <div class="ui-block-a">
          OC Amount : <xsl:value-of select="@InputPrice1Amount" />
        </div>
        <div class="ui-block-b">
          OC Unit : <xsl:value-of select="@InputUnit1" />
        </div>
        <div class="ui-block-a">
          OC Price : <xsl:value-of select="@InputPrice1" />
        </div>
        <div class="ui-block-b">
          OC : <xsl:value-of select="@TOC" />
        </div>
        <div class="ui-block-a">
          OC Int : <xsl:value-of select="@TOC_INT" />
        </div>
        <div class="ui-block-b">
          Total OC : <xsl:value-of select="DisplayDevPacks:GetAddNumberN2(@TOC, @TOC_INT, 0, 0)"/>
        </div>
        <div class="ui-block-a">
          AOH Amount : <xsl:value-of select="@InputPrice2Amount" />
        </div>
        <div class="ui-block-b">
          AOH Unit : <xsl:value-of select="@InputUnit2" />
        </div>
        <div class="ui-block-a">
          AOH Price : <xsl:value-of select="@InputPrice2" />
        </div>
        <div class="ui-block-b">
          AOH : <xsl:value-of select="@TAOH" />
        </div>
        <div class="ui-block-a">
          AOH Int : <xsl:value-of select="@TAOH_INT" />
        </div>
        <div class="ui-block-b">
          Total AOH : <xsl:value-of select="DisplayDevPacks:GetAddNumberN2(@TAOH, @TAOH_INT, 0, 0)"/>
        </div>
        <div class="ui-block-a">
          CAP Amount : <xsl:value-of select="@InputPrice3Amount" />
        </div>
        <div class="ui-block-b">
          CAP Unit : <xsl:value-of select="@InputUnit3" />
        </div>
        <div class="ui-block-a">
          CAP Price : <xsl:value-of select="@InputPrice3" />
        </div>
        <div class="ui-block-b">
          CAP : <xsl:value-of select="@TCAP" />
        </div>
        <div class="ui-block-a">
          CAP Int : <xsl:value-of select="@TCAP_INT" />
        </div>
        <div class="ui-block-b">
          Total CAP : <xsl:value-of select="DisplayDevPacks:GetAddNumberN2(@TCAP, @TCAP_INT, 0, 0)"/>
        </div>
        <div class="ui-block-a">
          Total Incent : <xsl:value-of select="@TINCENT" />
        </div>
        <div class="ui-block-b">
          Use AOH Only : <xsl:value-of select="@InputUseAOHOnly" />
        </div>
      </div>
		  <div >
			  Description : <xsl:value-of select="@Description" />
		  </div>
    </div>
	</xsl:template>
</xsl:stylesheet>
<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, January -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Output"
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
		<div id="mainwrapper">
			<table class="data" cellpadding="6" cellspacing="1" border="0">
				<tbody>
					<xsl:apply-templates select="servicebase" />
					<xsl:apply-templates select="outputgroup" />
					<tr id="footer">
						<td scope="row" colspan="10">
							<a id="aFeedback" name="Feedback">
								<xsl:attribute name="href">mailto:<xsl:value-of select="$clubEmail" />?subject=<xsl:value-of select="$selectedFileURIPattern" /></xsl:attribute>
								Feedback About <xsl:value-of select="$selectedFileURIPattern" />
							</a>
						</td>
					</tr>
				</tbody>
			</table>
		</div>
	</xsl:template>
	<xsl:template match="servicebase">
		<tr>
			<th colspan="10">
				Service: <xsl:value-of select="@Name" />
			</th>
		</tr>
		<xsl:apply-templates select="outputgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outputgroup">
		<tr>
			<th scope="col" colspan="10">
				Output Group
			</th>
		</tr>
		<tr>
			<td colspan="10">
				<strong><xsl:value-of select="@Name" /> </strong>
			</td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="output">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="output">
		<tr>
			<th scope="col" colspan="10"><strong>Output</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
    <tr>
			<td>
				<strong>
					Physical Health Rating
				</strong>
			</td>
			<td>
				<strong>
					Emotional Health Rating
				</strong>
			</td>
			<td>
				<strong>
					Social Health Rating
				</strong>
			</td>
			<td>
				<strong>
					Economic Health Rating
				</strong>
			</td>
      <td>
				<strong>
					Health Care Delivery Rating
				</strong>
			</td>
      <td>
				<strong>
					Average Benefit Rating
				</strong>
			</td>
      <td>
				<strong>
					Before Treatment QOL Rating
				</strong>
			</td>
      <td>
				<strong>
					After Treatment QOL Rating
				</strong>
			</td>
      <td>
				<strong>
					Before Treatment Years
				</strong>
			</td>
      <td>
				<strong>
					After Treatment Years
				</strong>
			</td>
    </tr>
    <tr>
			<td>
				<strong>
					Probability of After Treatment Years
				</strong>
			</td>
			<td>
				<strong>
					Equity Multiplier
				</strong>
			</td>
			<td>
				<strong>
					Quality Adjusted Life Years (QALY)
				</strong>
			</td>
      <td>
				<strong>
					Incremental QALY
				</strong>
			</td>
			<td>
				<strong>
					Time Tradeoff Years
				</strong>
			</td>
      <td>
				<strong>
					TTO QALY
				</strong>
			</td>
      <td>
				<strong>
					Output Cost
				</strong>
			</td>
			<td>
				<strong>
					Benefit Adjustment
				</strong>
			</td>
			<td>
				<strong>
					Adjusted Benefit
				</strong>
			</td>
      <td>
				<strong>
					Discount (real) Rate
				</strong>
			</td>
    </tr>
    <tr>
			<td>
				<strong>
					Output Effect1 Name
				</strong>
			</td>
			<td>
				<strong>
					Output Effect1 Unit
				</strong>
			</td>
			<td>
				<strong>
					Output Effect1 Amount
				</strong>
			</td>
			<td>
				<strong>
					Output Effect1 Price
				</strong>
			</td>
      <td>
				<strong>
					Output Effect1 Cost
				</strong>
			</td>
      <td>
				<strong>
					Age
				</strong>
			</td>
			<td>
				<strong>
					Gender
				</strong>
			</td>
			<td>
				<strong>
					Education Years
				</strong>
			</td>
      <td>
				<strong>
					Race
				</strong>
			</td>
      <td>
				<strong>
					Work Status
				</strong>
			</td>
    </tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="outputseries">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outputseries">
		<tr>
			<td scope="row" colspan="10">
				<strong>Output Series : <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName = 'outputgroup')">
      <tr>
			  <td>
					  <xsl:value-of select="@TPhysicalHealthRating" />
			  </td>
			  <td>
					  <xsl:value-of select="@TEmotionalHealthRating" />
			  </td>
			  <td>
					  <xsl:value-of select="@TSocialHealthRating" />
			  </td>
			  <td>
					  <xsl:value-of select="@TEconomicHealthRating" />
			  </td>
        <td>
					  <xsl:value-of select="@THealthCareDeliveryRating" />
			  </td>
        <td>
					  <xsl:value-of select="@TAverageBenefitRating" />
			  </td>
        <td>
					  <xsl:value-of select="@TBeforeQOLRating" />
			  </td>
        <td>
					  <xsl:value-of select="@TAfterQOLRating" />
			  </td>
        <td>
					  <xsl:value-of select="@TBeforeYears" />
			  </td>
        <td>
					  <xsl:value-of select="@TAfterYears" />
			  </td>
      </tr>
      <tr>
			  <td>
					  <xsl:value-of select="@TAfterYearsProb" />
			  </td>
			  <td>
					  <xsl:value-of select="@TEquityMultiplier" />
			  </td>
			  <td>
					  <xsl:value-of select="@TQALY" />
			  </td>
        <td>
					  <xsl:value-of select="@TICERQALY" />
			  </td>
			  <td>
					  <xsl:value-of select="@TTimeTradeoffYears" />
			  </td>
        <td>
					  <xsl:value-of select="@TTTOQALY" />
			  </td>
        <td>
					  <xsl:value-of select="@TOutputCost" />
			  </td>
			  <td>
					  <xsl:value-of select="@TBenefitAdjustment" />
			  </td>
			  <td>
					  <xsl:value-of select="@TAdjustedBenefit" />
			  </td>
        <td>
					  <xsl:value-of select="@TRealRate" />
			  </td>
      </tr>
		</xsl:if>
		<xsl:if test="($localName != 'outputgroup')">
       <tr>
        <td>
					  <xsl:value-of select="@PhysicalHealthRating" />
			  </td>
			  <td>
					  <xsl:value-of select="@EmotionalHealthRating" />
			  </td>
			  <td>
					  <xsl:value-of select="@SocialHealthRating" />
			  </td>
			  <td>
					  <xsl:value-of select="@EconomicHealthRating" />
			  </td>
        <td>
					  <xsl:value-of select="@HealthCareDeliveryRating" />
			  </td>
        <td>
					  <xsl:value-of select="@AverageBenefitRating" />
			  </td>
        <td>
					  <xsl:value-of select="@BeforeQOLRating" />
			  </td>
        <td>
					  <xsl:value-of select="@AfterQOLRating" />
			  </td>
        <td>
					  <xsl:value-of select="@BeforeYears" />
			  </td>
        <td>
					  <xsl:value-of select="@AfterYears" />
			  </td>
      </tr>
      <tr>
			  <td>
					  <xsl:value-of select="@AfterYearsProb" />
			  </td>
			  <td>
					  <xsl:value-of select="@EquityMultiplier" />
			  </td>
			  <td>
					  <xsl:value-of select="@QALY" />
			  </td>
        <td>
					  <xsl:value-of select="@ICERQALY" />
			  </td>
			  <td>
					  <xsl:value-of select="@TimeTradeoffYears" />
			  </td>
        <td>
					  <xsl:value-of select="@TTOQALY" />
			  </td>
        <td>
					  <xsl:value-of select="@OutputCost" />
			  </td>
			  <td>
					  <xsl:value-of select="@BenefitAdjustment" />
			  </td>
			  <td>
					  <xsl:value-of select="@AdjustedBenefit" />
			  </td>
        <td>
					  <xsl:value-of select="@RealRate" />
			  </td>
      </tr>
      <tr>
			  <td>
					  <xsl:value-of select="@OutputEffect1Name" />
			  </td>
			  <td>
					  <xsl:value-of select="@OutputEffect1Unit" />
			  </td>
			  <td>
					  <xsl:value-of select="@OutputEffect1Amount" />
			  </td>
			  <td>
					  <xsl:value-of select="@OutputEffect1Price" />
			  </td>
        <td>
					  <xsl:value-of select="@OutputEffect1Cost" />
			  </td>
        <td>
					  <xsl:value-of select="@Age" />
			  </td>
			  <td>
					  <xsl:value-of select="@Gender" />
			  </td>
			  <td>
					  <xsl:value-of select="@EducationYears" />
			  </td>
        <td>
					  <xsl:value-of select="@Race" />
			  </td>
        <td>
					  <xsl:value-of select="@WorkStatus" />
			  </td>
      </tr>
      <tr>
        <td colspan="10">
          <strong>Benefit Assessment :</strong> <xsl:value-of select="@BenefitAssessment" />
			  </td>
      </tr>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>


<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2012, December -->
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
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
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
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgettimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgettimeperiod">
		<h4>
      <strong>Time Period</strong> : <xsl:value-of select="@EnterpriseName" />
    </h4>
    <div>
      <strong>Outcomes</strong>
    </div>
		<xsl:apply-templates select="budgetoutcomes" />
    <div>
      <strong>Operations</strong>
    </div>
		<xsl:apply-templates select="budgetoperations" />
    <h4>
      Time Period Totals
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
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
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgetoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutput">
    <h4>
      <strong>Output </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
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
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgetinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetinput">
		<h4>
      <strong>Input </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
		<xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName != 'budgetinput' and $localName != 'budgetoutput')">
      <xsl:if test="($localName != 'budgetoperation')">
        <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Benefits</strong>
        </h4>
      </div>
      </xsl:if>
      <xsl:if test="($localName != 'budgetoutcome')">
        <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Costs</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Container Size : <xsl:value-of select="@TContainerSize" />
          </div>
          <div class="ui-block-b">
            Serving Cost : <xsl:value-of select="@TServingCost" />
          </div>
          <div class="ui-block-a">
            Servings Per Container : <xsl:value-of select="@TServingsPerContainer" />
          </div>
          <div class="ui-block-b">
            Actual Servings Per Container : <xsl:value-of select="@TActualServingsPerContainer" />
          </div>
          <div class="ui-block-a">
           Gender Of Serving Person : <xsl:value-of select="@TGenderOfServingPerson" />
          </div>
          <div class="ui-block-b">
           Weight Of Serving Person : <xsl:value-of select="@TWeightOfServingPerson" />
          </div>
          <div class="ui-block-a">
           Actual Calories Per Day : <xsl:value-of select="@TActualCaloriesPerDay" />
          </div>
          <div class="ui-block-b">
           Calories Per Actual Serving : <xsl:value-of select="@TCaloriesPerActualServing" />
          </div>
          <div class="ui-block-a">
            Calories From Fat Per Actual Serving : <xsl:value-of select="@TCaloriesFromFatPerActualServing" />
          </div>
          <div class="ui-block-b">
            Total Fat Per Actual Serving : <xsl:value-of select="@TTotalFatPerActualServing" />
          </div>
          <div class="ui-block-a">
            Total Fat Actual Daily Percent :  <xsl:value-of select="@TTotalFatActualDailyPercent" />
          </div>
          <div class="ui-block-b">
           Saturated Fat Per Actual Serving : <xsl:value-of select="@TSaturatedFatPerActualServing" />
          </div>
          <div class="ui-block-a">
            Saturated Fat Actual Daily Percent : <xsl:value-of select="@TSaturatedFatActualDailyPercent" />
          </div>
          <div class="ui-block-b">
            Trans Fat Per Actual Serving : <xsl:value-of select="@TTransFatPerActualServing" />
          </div>
          <div class="ui-block-a">
            Cholesterol Per Actual Serving : <xsl:value-of select="@TCholesterolPerActualServing" />
          </div>
          <div class="ui-block-b">
           Cholesterol Actual Daily Percent : <xsl:value-of select="@TCholesterolActualDailyPercent" />
          </div>
          <div class="ui-block-a">
            Sodium Per Actual Serving : <xsl:value-of select="@TSodiumPerActualServing" />
          </div>
          <div class="ui-block-b">
            Sodium Actual Daily Percent : <xsl:value-of select="@TSodiumActualDailyPercent" />
          </div>
          <div class="ui-block-a">
            Potassium Per Actual Serving : <xsl:value-of select="@TPotassiumPerActualServing" />
          </div>
          <div class="ui-block-b">
            Total Carbohydrate Per Actual Serving : <xsl:value-of select="@TTotalCarbohydratePerActualServing" />
          </div>
          <div class="ui-block-a">
            Total Carbohydrate Actual Daily Percent : <xsl:value-of select="@TTotalCarbohydrateActualDailyPercent" />
          </div>
          <div class="ui-block-b">
           Other Carbohydrate Per Actual Serving : <xsl:value-of select="@TOtherCarbohydratePerActualServing" />
          </div>
          <div class="ui-block-a">
           Other Carbohydrate Actual Daily Percent : <xsl:value-of select="@TOtherCarbohydrateActualDailyPercent" />
          </div>
          <div class="ui-block-b">
           Dietary Fiber Per Actual Serving : <xsl:value-of select="@TDietaryFiberPerActualServing" />
          </div>
          <div class="ui-block-a">
            Dietary Fiber Actual Daily Percent : <xsl:value-of select="@TDietaryFiberActualDailyPercent" />
          </div>
          <div class="ui-block-b">
            Sugars Per Actual Serving : <xsl:value-of select="@TSugarsPerActualServing" />
          </div>
          <div class="ui-block-a">
            Protein Per Actual Serving : <xsl:value-of select="@TProteinPerActualServing" />
          </div>
          <div class="ui-block-b">
           Protein Actual Daily Percent : <xsl:value-of select="@TProteinActualDailyPercent" />
          </div>
          <div class="ui-block-a">
            Vitamin A Percent Actual Daily Value : <xsl:value-of select="@TVitaminAPercentActualDailyValue" />
          </div>
        </div>
      </div>
      </xsl:if>
		</xsl:if>
		<xsl:if test="($localName = 'budgetinput')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Input Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Container Size : <xsl:value-of select="@ContainerSize" />
          </div>
          <div class="ui-block-b">
            Serving Cost : <xsl:value-of select="@ServingCost" />
          </div>
          <div class="ui-block-a">
            Servings Per Container : <xsl:value-of select="@ServingsPerContainer" />
          </div>
          <div class="ui-block-b">
            Actual Servings Per Container : <xsl:value-of select="@ActualServingsPerContainer" />
          </div>
          <div class="ui-block-a">
           Gender Of Serving Person : <xsl:value-of select="@GenderOfServingPerson" />
          </div>
          <div class="ui-block-b">
           Weight Of Serving Person : <xsl:value-of select="@WeightOfServingPerson" />
          </div>
          <div class="ui-block-a">
           Actual Calories Per Day : <xsl:value-of select="@ActualCaloriesPerDay" />
          </div>
          <div class="ui-block-b">
           Calories Per Actual Serving : <xsl:value-of select="@CaloriesPerActualServing" />
          </div>
          <div class="ui-block-a">
            Calories From Fat Per Actual Serving : <xsl:value-of select="@CaloriesFromFatPerActualServing" />
          </div>
          <div class="ui-block-b">
            Total Fat Per Actual Serving : <xsl:value-of select="@TotalFatPerActualServing" />
          </div>
          <div class="ui-block-a">
            Total Fat Actual Daily Percent :  <xsl:value-of select="@TotalFatActualDailyPercent" />
          </div>
          <div class="ui-block-b">
           Saturated Fat Per Actual Serving : <xsl:value-of select="@SaturatedFatPerActualServing" />
          </div>
          <div class="ui-block-a">
            Saturated Fat Actual Daily Percent : <xsl:value-of select="@SaturatedFatActualDailyPercent" />
          </div>
          <div class="ui-block-b">
            Trans Fat Per Actual Serving : <xsl:value-of select="@TransFatPerActualServing" />
          </div>
          <div class="ui-block-a">
            Cholesterol Per Actual Serving : <xsl:value-of select="@CholesterolPerActualServing" />
          </div>
          <div class="ui-block-b">
           Cholesterol Actual Daily Percent : <xsl:value-of select="@CholesterolActualDailyPercent" />
          </div>
          <div class="ui-block-a">
            Sodium Per Actual Serving : <xsl:value-of select="@SodiumPerActualServing" />
          </div>
          <div class="ui-block-b">
            Sodium Actual Daily Percent : <xsl:value-of select="@SodiumActualDailyPercent" />
          </div>
          <div class="ui-block-a">
            Potassium Per Actual Serving : <xsl:value-of select="@PotassiumPerActualServing" />
          </div>
          <div class="ui-block-b">
            Total Carbohydrate Per Actual Serving : <xsl:value-of select="@TotalCarbohydratePerActualServing" />
          </div>
          <div class="ui-block-a">
            Total Carbohydrate Actual Daily Percent : <xsl:value-of select="@TotalCarbohydrateActualDailyPercent" />
          </div>
          <div class="ui-block-b">
           Other Carbohydrate Per Actual Serving : <xsl:value-of select="@OtherCarbohydratePerActualServing" />
          </div>
          <div class="ui-block-a">
           Other Carbohydrate Actual Daily Percent : <xsl:value-of select="@OtherCarbohydrateActualDailyPercent" />
          </div>
          <div class="ui-block-b">
           Dietary Fiber Per Actual Serving : <xsl:value-of select="@DietaryFiberPerActualServing" />
          </div>
          <div class="ui-block-a">
            Dietary Fiber Actual Daily Percent : <xsl:value-of select="@DietaryFiberActualDailyPercent" />
          </div>
          <div class="ui-block-b">
            Sugars Per Actual Serving : <xsl:value-of select="@SugarsPerActualServing" />
          </div>
          <div class="ui-block-a">
            Protein Per Actual Serving : <xsl:value-of select="@ProteinPerActualServing" />
          </div>
          <div class="ui-block-b">
           Protein Actual Daily Percent : <xsl:value-of select="@ProteinActualDailyPercent" />
          </div>
          <div class="ui-block-a">
            Vitamin A Percent Actual Daily Value : <xsl:value-of select="@VitaminAPercentActualDailyValue" />
          </div>
        </div>
         <div >
			    <strong>Input Quality Assessment : </strong><xsl:value-of select="@InputQualityAssessment" />
	      </div>
         <div >
			    <strong>Additional Costs Description : </strong><xsl:value-of select="@AdditionalCostsDescription" />
	      </div>
      </div>
		</xsl:if>
    <xsl:if test="($localName = 'budgetoutput')">
			<div data-role="collapsible"  data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Output Details</strong>
        </h4>
        
      </div>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>

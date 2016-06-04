<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, October -->
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
		<div id="mainwrapper">
			<table class="data" cellpadding="6" cellspacing="1" border="0">
				<tbody>
					<xsl:apply-templates select="servicebase" />
					<xsl:apply-templates select="outcomegroup" />
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
		<xsl:apply-templates select="outcomegroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcomegroup">
		<tr>
			<th scope="col" colspan="10">
				Outcome Group
			</th>
		</tr>
		<tr>
			<td colspan="10">
				<strong><xsl:value-of select="@Name" /> </strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcatotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="outcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcome">
      <tr>
			  <th scope="col" colspan="10"><strong>Outcome</strong></th>
		  </tr>
		  <tr>
			  <td scope="row" colspan="10">
				  <strong><xsl:value-of select="@Name" /></strong>
			  </td>
		  </tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcatotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:variable name="outcount" select="count(outcomeoutput)"/>
    <xsl:if test="($outcount > 0)">
      <tr>
        <td>
				  <strong>
					  SubBenefit Name
				  </strong>
			  </td>
        <td>
				  <strong>
					  SubBenefit Amount
				  </strong>
			  </td>
        <td>
				  <strong>
					  SubBenefit Unit
				  </strong>
			  </td>
        <td>
				  <strong>
					  SubBenefit Price
				  </strong>
			  </td>
			  <td>
				  <strong>
					  SubBenefit Total
				  </strong>
			  </td>
        <td>
				  <strong>
					  SubBenefit Unit Total
				  </strong>
			  </td>
			  <td>
          <strong>
					  SubBenefit Label
				  </strong>
        </td>
			  <td colspan="3">
			  </td>
		  </tr>
     </xsl:if>
    <xsl:apply-templates select="outcomeoutput">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcomeoutput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Output : <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcatotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName != 'outcomeoutput')">
      <tr>
        <td>
				  <strong>
					  SubBenefit Name
				  </strong>
			  </td>
        <td>
				  <strong>
					  SubBenefit Amount
				  </strong>
			  </td>
        <td>
				  <strong>
					  SubBenefit Unit
				  </strong>
			  </td>
        <td>
				  <strong>
					  SubBenefit Price
				  </strong>
			  </td>
			  <td>
				  <strong>
					  SubBenefit Total
				  </strong>
			  </td>
        <td>
				  <strong>
					  SubBenefit Unit Total
				  </strong>
			  </td>
			  <td>
          <strong>
					  SubBenefit Label
				  </strong>
        </td>
			  <td colspan="3">
			  </td>
		  </tr>
		  <xsl:if test="(@TSubP2Name1 != '' and @TSubP2Name1 != 'none')">
		    <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount1"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit1"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label1"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description1"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name2 != '' and @TSubP2Name2 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount2"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit2"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label2"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description2"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name3 != '' and @TSubP2Name3 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount3"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit3"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label3"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description3"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name4 != '' and @TSubP2Name4 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount4"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit4"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label4"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description4"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name5 != '' and @TSubP2Name5 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount5"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit5"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label5"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description5"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name6 != '' and @TSubP2Name6 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount6"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit6"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label6"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description6"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name7 != '' and @TSubP2Name7 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount7"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit7"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label7"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description7"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name8 != '' and @TSubP2Name8 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount8"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit8"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label8"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description8"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name9 != '' and @TSubP2Name9 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount9"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit9"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label9"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description9"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name10 != '' and @TSubP2Name10 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount10"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit10"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label10"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description2"/>
			    </td>
		    </tr>
      </xsl:if>
      <tr>
        <td>
				
			  </td>
			  <td>
				
			  </td>
			  <td>
				  <strong>
            Total Revenue
				  </strong>
			  </td>
			  <td>
				  <strong>
            Total EAA
				  </strong>
			  </td>
        <td>
				  <strong>
					  Total LCB
				  </strong>
			  </td>
        <td>
				  <strong>
            Total Unit Benefit
				  </strong>
			  </td>
        <td>
				
			  </td>
        <td>
				
			  </td>
			  <td>
				
			  </td>
			  <td>
				
			  </td>
		  </tr>
      <tr>
        <td>
          Totals
			  </td>
        <td>
          ---
			  </td>
			  <td>
          <xsl:value-of select="@TRBenefit"/>
			  </td>
        <td>
          <xsl:value-of select="@TREAABenefit"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TLCBBenefit"/>
			  </td>
			  <td>
          <xsl:value-of select="@TRUnitBenefit"/>
			  </td>
			  <td colspan="4">
			  </td>
		  </tr>
		</xsl:if>
		<xsl:if test="($localName = 'outcomeoutput')">
		  <xsl:if test="(@SubPName1 != '' and @SubPName1 != 'none')">
		    <tr>
          <td>
					    <xsl:value-of select="@SubPName1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPAmount1"/>
			    </td>
          <td>
					    <xsl:value-of select="@SubPUnit1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPPrice1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotal1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotalPerUnit1"/>
			    </td>
			    <td>
              <xsl:value-of select="@SubPLabel1"/>
			    </td>
		      <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SubPDescription1"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@SubPName2 != '' and @SubPName2 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@SubPName2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPAmount2"/>
			    </td>
          <td>
					    <xsl:value-of select="@SubPUnit2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPPrice2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotal2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotalPerUnit2"/>
			    </td>
			    <td>
              <xsl:value-of select="@SubPLabel2"/>
			    </td>
			     <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SubPDescription2"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@SubPName3 != '' and @SubPName3 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@SubPName3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPAmount3"/>
			    </td>
          <td>
					    <xsl:value-of select="@SubPUnit3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPPrice3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotal3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotalPerUnit3"/>
			    </td>
			    <td>
              <xsl:value-of select="@SubPLabel3"/>
			    </td>
			     <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SubPDescription3"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@SubPName4 != '' and @SubPName4 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@SubPName4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPAmount4"/>
			    </td>
          <td>
					    <xsl:value-of select="@SubPUnit4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPPrice4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotal4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotalPerUnit4"/>
			    </td>
			    <td>
              <xsl:value-of select="@SubPLabel4"/>
			    </td>
			     <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SubPDescription4"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@SubPName5 != '' and @SubPName5 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@SubPName5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPAmount5"/>
			    </td>
          <td>
					    <xsl:value-of select="@SubPUnit5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPPrice5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotal5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotalPerUnit5"/>
			    </td>
			    <td>
              <xsl:value-of select="@SubPLabel5"/>
			    </td>
			     <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SubPDescription5"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@SubPName6 != '' and @SubPName6 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@SubPName6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPAmount6"/>
			    </td>
          <td>
					    <xsl:value-of select="@SubPUnit6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPPrice6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotal6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotalPerUnit6"/>
			    </td>
			    <td>
              <xsl:value-of select="@SubPLabel6"/>
			    </td>
			     <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SubPDescription6"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@SubPName7 != '' and @SubPName7 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@SubPName7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPAmount7"/>
			    </td>
          <td>
					    <xsl:value-of select="@SubPUnit7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPPrice7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotal7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotalPerUnit7"/>
			    </td>
			    <td>
              <xsl:value-of select="@SubPLabel7"/>
			    </td>
			     <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SubPDescription7"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@SubPName8 != '' and @SubPName8 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@SubPName8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPAmount8"/>
			    </td>
          <td>
					    <xsl:value-of select="@SubPUnit8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPPrice8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotal8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotalPerUnit8"/>
			    </td>
			    <td>
              <xsl:value-of select="@SubPLabel8"/>
			    </td>
			     <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SubPDescription8"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@SubPName9 != '' and @SubPName9 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@SubPName9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPAmount9"/>
			    </td>
          <td>
					    <xsl:value-of select="@SubPUnit9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPPrice9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotal9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotalPerUnit9"/>
			    </td>
			    <td>
              <xsl:value-of select="@SubPLabel9"/>
			    </td>
			     <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SubPDescription9"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@SubPName10 != '' and @SubPName10 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@SubPName10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPAmount10"/>
			    </td>
          <td>
					    <xsl:value-of select="@SubPUnit10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPPrice10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotal10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@SubPTotalPerUnit10"/>
			    </td>
			    <td>
              <xsl:value-of select="@SubPLabel10"/>
			    </td>
			     <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@SubPDescription10"/>
			    </td>
		    </tr>
      </xsl:if>
      <tr>
      <td>
				<strong>
					Unit Amount
				</strong>
			</td>
			<td>
				<strong>
					Unit
				</strong>
			</td>
			<td>
				<strong>
          Total Revenue
				</strong>
			</td>
			<td>
				<strong>
          Total EAA
				</strong>
			</td>
      <td>
				<strong>
					Total LCB
				</strong>
			</td>
      <td>
				<strong>
          Total Unit Benefit
				</strong>
			</td>
      <td>
				<strong>
					Service Life
				</strong>
			</td>
      <td>
				<strong>
					P/C Years
				</strong>
			</td>
			<td>
				<strong>
					Yrs From Base Date
				</strong>
			</td>
			<td>
				<strong>
          Target Type / AlternType
				</strong>
			</td>
		</tr>
      <tr>
        <td>
					  <xsl:value-of select="@PerUnitAmount"/>
			  </td>
			  <td>
					  <xsl:value-of select="@PerUnitUnit"/>
			  </td>
			  <td>
          <xsl:value-of select="@RTotalBenefit"/>
			  </td>
			  <td>
          <xsl:value-of select="@EAATotalBenefit"/>
			  </td>
        <td>
				   <xsl:value-of select="@LCBTotalBenefit"/>
			  </td>
        <td>
					  <xsl:value-of select="@UnitTotalBenefit"/>
			  </td>
        <td>
					  <xsl:value-of select="@ServiceLifeYears"/>
			  </td>
			  <td>
					  <xsl:value-of select="@PlanningConstructionYears"/>
			  </td>
			  <td>
					  <xsl:value-of select="@YearsFromBaseDate"/>
			  </td>
        <td>
            <xsl:value-of select="@AlternativeType"/>; <xsl:value-of select="@TargetType"/>
			  </td>
		  </tr>
      <tr>
			  <td>
          <xsl:value-of select="@RTotalBenefit"/>
			  </td>
			  <td>
				   <xsl:value-of select="@LCBTotalBenefit"/>
			  </td>
			  <td>
          <xsl:value-of select="@EAATotalBenefit"/>
			  </td>
        <td>
					  <xsl:value-of select="@UnitTotalBenefit"/>
			  </td>
			   <td>
					  <xsl:value-of select="@PerUnitAmount"/>
			  </td>
			  <td>
					  <xsl:value-of select="@PerUnitUnit"/>
			  </td>
        <td>
					  <xsl:value-of select="@ServiceLifeYears"/>
			  </td>
			  <td>
					  <xsl:value-of select="@PlanningConstructionYears"/>
			  </td>
			  <td>
					  <xsl:value-of select="@YearsFromBaseDate"/>
			  </td>
        <td>
            <xsl:value-of select="@AlternativeType"/>; <xsl:value-of select="@TargetType"/>
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong><xsl:value-of select="@CalculatorDescription" />
			  </td>
      </tr>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
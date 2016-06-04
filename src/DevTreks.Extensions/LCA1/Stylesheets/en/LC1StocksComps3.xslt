<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, October -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Component"
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
					<xsl:apply-templates select="componentgroup" />
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
		<xsl:apply-templates select="componentgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="componentgroup">
		<tr>
			<th scope="col" colspan="10">
				Component Group
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
    <xsl:apply-templates select="component">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="component">
    <tr>
			<th scope="col" colspan="10"><strong>Component</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcatotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:variable name="incount" select="count(componentinput)"/>
    <xsl:if test="($incount > 0)">
      <tr>
        <td>
				  <strong>
					  SubCost Name
				  </strong>
			  </td>
        <td>
				  <strong>
					  SubCost Amount
				  </strong>
			  </td>
        <td>
				  <strong>
					  SubCost Unit
				  </strong>
			  </td>
        <td>
				  <strong>
					  SubCost Price
				  </strong>
			  </td>
			  <td>
				  <strong>
					  SubCost Total
				  </strong>
			  </td>
        <td>
				  <strong>
					  SubCost Unit Total
				  </strong>
			  </td>
        <td>
          <strong>
					  SubCost Label
				  </strong>
        </td>
			  <td colspan="3">
			  </td>
		  </tr>
    </xsl:if>
    <xsl:apply-templates select="componentinput">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="componentinput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Input: <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcatotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName != 'componentinput')">
      <tr>
      <td>
				<strong>
					SubCost Name
				</strong>
			</td>
      <td>
				<strong>
					SubCost Amount
				</strong>
			</td>
      <td>
				<strong>
					SubCost Unit
				</strong>
			</td>
      <td>
				<strong>
					SubCost Price
				</strong>
			</td>
			<td>
				<strong>
					SubCost Total
				</strong>
			</td>
      <td>
				<strong>
					SubCost Unit Total
				</strong>
			</td>
      <td>
        <strong>
					SubCost Label
				</strong>
      </td>
			<td colspan="3">
			</td>
		</tr>
      <xsl:if test="(@TSubP1Name1 != '' and @TSubP1Name1 != 'none' )">
		    <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount1"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit1"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label1"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description1"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name2 != '' and @TSubP1Name2 != 'none' )">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount2"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit2"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label2"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description2"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name3 != '' and @TSubP1Name3 != 'none' )">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount3"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit3"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label3"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description3"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name4 != '' and @TSubP1Name4 != 'none' )">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount4"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit4"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label4"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description4"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name5 != '' and @TSubP1Name5 != 'none' )">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount5"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit5"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label5"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description5"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name6 != '' and @TSubP1Name6 != 'none' )">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount6"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit6"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label6"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description6"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name7 != '' and @TSubP1Name7 != 'none' )">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount7"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit7"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label7"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description7"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name8 != '' and @TSubP1Name8 != 'none' )">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount8"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit8"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label8"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description8"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name9 != '' and @TSubP1Name9 != 'none' )">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount9"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit9"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label9"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description9"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name10 != '' and @TSubP1Name10 != 'none' )">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount10"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit10"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label10"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description10"/>
			    </td>
		    </tr>
      </xsl:if>
      <tr>
        <td>
			  </td>
			  <td>
				  <strong>
					  Total OC
				  </strong>
			  </td>
			  <td>
				  <strong>
					  Total AOH
				  </strong>
			  </td>
			  <td>
				  <strong>
					  Total CAP
				  </strong>
			  </td>
			  <td>
				  <strong>
					  Total LCC
				  </strong>
			  </td>
        <td>
				  <strong>
					  Total Unit
				  </strong>
			  </td>
        <td>
				  <strong>
					  Total EAA
				  </strong>
			  </td>
        <td colspan="3">
			  </td>
		  </tr>
      <tr>
        <td>
          Totals
			  </td>
			  <td>
				  <xsl:value-of select="@TOCCost"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TAOHCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TCAPCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TLCCCost"/>
			  </td>
        <td>
					  <xsl:value-of select="@TUnitCost"/>
			  </td>
        <td>
					  <xsl:value-of select="@TEAACost"/>
			  </td>
        <td colspan="3">
			  </td>
		  </tr>
		</xsl:if>
		<xsl:if test="($localName = 'componentinput')">
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
			</td>
			<td>
				<strong>
					Total OC
				</strong>
			</td>
			<td>
				<strong>
					Total AOH
				</strong>
			</td>
			<td>
				<strong>
					Total CAP
				</strong>
			</td>
			<td>
				<strong>
					Total LCC
				</strong>
			</td>
      <td>
				<strong>
					Total Unit
				</strong>
			</td>
      <td>
				<strong>
					Total EAA
				</strong>
			</td>
      <td colspan="3">
			</td>
		</tr>
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
          Total Unit Cost
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
          Altern Type
				  </strong>
			</td>
			<td>
				<strong>
        Target Type
				</strong>
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
				  <xsl:value-of select="@OCTotalCost"/>
			  </td>
			  <td>
				  <xsl:value-of select="@AOHTotalCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@CAPTotalCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@LCCTotalCost"/>
			  </td>
        <td>
					  <xsl:value-of select="@UnitTotalCost"/>
			  </td>
        <td>
					  <xsl:value-of select="@EAATotalCost"/>
			  </td>
        <td colspan="3">
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
					  <xsl:value-of select="@UnitTotalCost"/>
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
					  <xsl:value-of select="@AlternativeType"/>
			  </td>
        <td>
					  <xsl:value-of select="@TargetType"/>
			  </td>
			  <td>
			  </td>
        <td>
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

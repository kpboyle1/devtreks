﻿<?xml version="1.0" encoding="UTF-8" ?>
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
    <tr>
      <th>
				Cost or B Type
			</th>
      <th>
				Total
			</th>
			<th>
				Amount Change
			</th>
			<th>
				Percent Change
			</th>
      <th>
				Base Change
			</th>
			<th>
				Base Percent Change
			</th>
      <th>
			</th>
			<th>
			</th>
      <th>
			</th>
			<th>
			</th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcachangeyr' 
      or @AnalyzerType='lcachangeid' or @AnalyzerType='lcachangealt']">
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
				<strong><xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" /></strong>
			</td>
		</tr>
    <tr>
      <th>
				Cost or B Type
			</th>
      <th>
				Total
			</th>
			<th>
				Amount Change
			</th>
			<th>
				Percent Change
			</th>
      <th>
				Base Change
			</th>
			<th>
				Base Percent Change
			</th>
      <th>
			</th>
			<th>
			</th>
      <th>
			</th>
			<th>
			</th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcachangeyr' 
      or @AnalyzerType='lcachangeid' or @AnalyzerType='lcachangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:variable name="count" select="count(outcomeoutput)"/>
    <xsl:if test="($count > 0)">
      <tr>
      <th>
				Cost or B Type
			</th>
      <th>
				Total
			</th>
			<th>
				Amount Change
			</th>
			<th>
				Percent Change
			</th>
      <th>
				Base Change
			</th>
			<th>
				Base Percent Change
			</th>
      <th>
			</th>
			<th>
			</th>
      <th>
			</th>
			<th>
			</th>
		</tr>
      <xsl:apply-templates select="outcomeoutput">
			  <xsl:sort select="@OutputDate"/>
		  </xsl:apply-templates>
    </xsl:if>
	</xsl:template>
	<xsl:template match="outcomeoutput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Output: <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcachangeyr' 
      or @AnalyzerType='lcachangeid' or @AnalyzerType='lcachangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName != 'outcomeoutput')">
      <tr>
			  <td colspan="10">
				  Date : <xsl:value-of select="@Date"/> ; Observations : <xsl:value-of select="@Observations"/>; Alternative : <xsl:value-of select="@AlternativeType"/>
			  </td>
      </tr>
			<tr>
			  <td>
				  Ben
			  </td>
			  <td>
				  <xsl:value-of select="@TRBenefit"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRAmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRPercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRBaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TRBasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  LCB
			  </td>
			  <td>
				  <xsl:value-of select="@TLCBBenefit"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TLCBAmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TLCBPercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TLCBBaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TLCBBasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  Unit
			  </td>
			  <td>
				  <xsl:value-of select="@TRUnitBenefit"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRUnitAmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRUnitPercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRUnitBaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TRUnitBasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  REAA
			  </td>
			  <td>
				  <xsl:value-of select="@TREAABenefit"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TREAAAmountChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TREAAPercentChange"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TREAABaseChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TREAABasePercentChange"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
		</xsl:if>
		<xsl:if test="($localName = 'outcomeoutput')">
      <tr>
        <td>
          Total Benefit:
			  </td>
			  <td>
				  <xsl:value-of select="@TRBenefit"/>
			  </td>
        <td>
          Life Cycle Bs:
			  </td>
			  <td>
				  <xsl:value-of select="@TLCBBenefit"/>
			  </td>
        <td>
          Unit Benefits:
			  </td>
			  <td>
					  <xsl:value-of select="@TRUnitBenefit"/>
			  </td>
        <td>
          Annual Benefits:
			  </td>
			  <td>
					  <xsl:value-of select="@TREAABenefit"/>
			  </td>
        <td colspan="2">
			  </td>
		  </tr>
		</xsl:if>
    <tr>
      <td>
				<strong>
					SubBOrC Name
				</strong>
			</td>
      <td>
				<strong>
					SubBOrC Amount
				</strong>
			</td>
      <td>
				<strong>
					SubBOrC Unit
				</strong>
			</td>
      <td>
				<strong>
					SubBOrC Price
				</strong>
			</td>
			<td>
				<strong>
					SubBOrC Total
				</strong>
			</td>
      <td>
				<strong>
					SubBOrC Unit Total
				</strong>
			</td>
      <td>
        <strong>
					SubBOrC Label
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
          <xsl:value-of select="@TSubP2Description10"/>
			  </td>
		  </tr>
    </xsl:if>
	</xsl:template>
</xsl:stylesheet>

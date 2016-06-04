﻿<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, December -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Input"
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
			<xsl:apply-templates select="inputgroup" />
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
		<xsl:apply-templates select="inputgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="inputgroup">
		<h4>
      <strong>Input Group</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvchangeyr' 
      or @AnalyzerType='npvchangeid' or @AnalyzerType='npvchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="input">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="input">
    <h4>
      <strong>Input</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvchangeyr' 
      or @AnalyzerType='npvchangeid' or @AnalyzerType='npvchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="inputseries">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="inputseries">
		<h4>
      <strong>Input Series</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvchangeyr' 
      or @AnalyzerType='npvchangeid' or @AnalyzerType='npvchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName = 'inputgroup')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Input Group Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Date : <xsl:value-of select="@Date"/>
          </div>
          <div class="ui-block-b">
            Observations : <xsl:value-of select="@Observations"/>; Alternative : <xsl:value-of select="@AlternativeType"/>
          </div>
          <div class="ui-block-a">
            OC Total : <xsl:value-of select="@TAMOC"/>
          </div>
          <div class="ui-block-b">
            OC AmountChange : <xsl:value-of select="@TAMOCAmountChange"/>
          </div>
          <div class="ui-block-a">
            OC PercentChange : <xsl:value-of select="@TAMOCPercentChange"/>
          </div>
          <div class="ui-block-b">
            OC BaseChange : <xsl:value-of select="@TAMOCBaseChange"/>
          </div>
          <div class="ui-block-a">
            OC BasePercentChange : <xsl:value-of select="@TAMOCBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            AOH Total : <xsl:value-of select="@TAMAOH"/>
          </div>
          <div class="ui-block-b">
            AOH AmountChange : <xsl:value-of select="@TAMAOHAmountChange"/>
          </div>
          <div class="ui-block-a">
            AOH PercentChange : <xsl:value-of select="@TAMAOHPercentChange"/>
          </div>
          <div class="ui-block-b">
            AOH BaseChange : <xsl:value-of select="@TAMAOHBaseChange"/>
          </div>
          <div class="ui-block-a">
            AOH BasePercentChange : <xsl:value-of select="@TAMAOHBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            CAP Total : <xsl:value-of select="@TAMCAP"/>
          </div>
          <div class="ui-block-b">
            CAP AmountChange : <xsl:value-of select="@TAMCAPAmountChange"/>
          </div>
          <div class="ui-block-a">
            CAP PercentChange : <xsl:value-of select="@TAMCAPPercentChange"/>
          </div>
          <div class="ui-block-b">
            CAP BaseChange : <xsl:value-of select="@TAMCAPBaseChange"/>
          </div>
          <div class="ui-block-a">
            CAP BasePercentChange : <xsl:value-of select="@TAMCAPBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            Total Total : <xsl:value-of select="@TAMTOTAL"/>
          </div>
          <div class="ui-block-b">
            Total AmountChange : <xsl:value-of select="@TAMAmountChange"/>
          </div>
          <div class="ui-block-a">
            Total PercentChange : <xsl:value-of select="@TAMPercentChange"/>
          </div>
          <div class="ui-block-b">
            Total BaseChange : <xsl:value-of select="@TAMBaseChange"/>
          </div>
          <div class="ui-block-a">
            Total BasePercentChange : <xsl:value-of select="@TAMBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
        </div>
      </div>
		</xsl:if>
		<xsl:if test="($localName != 'inputgroup')">
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Input Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
           Date : <xsl:value-of select="@Date"/>
          </div>
          <div class="ui-block-b">
            Observations : <xsl:value-of select="@Observations"/>; Alternative : <xsl:value-of select="@AlternativeType"/>
          </div>
          <div class="ui-block-a">
            OC Total : <xsl:value-of select="@TAMOC"/>
          </div>
          <div class="ui-block-b">
            OC Amount Change : <xsl:value-of select="@TAMOCAmountChange"/>
          </div>
          <div class="ui-block-a">
            OC Percent Change : <xsl:value-of select="@TAMOCPercentChange"/>
          </div>
          <div class="ui-block-b">
            OC Base Change : <xsl:value-of select="@TAMOCBaseChange"/>
          </div>
          <div class="ui-block-a">
            OC Base Percent Change : <xsl:value-of select="@TAMOCBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            AOH Total : <xsl:value-of select="@TAMAOH"/>
          </div>
          <div class="ui-block-b">
            AOH AmountChange : <xsl:value-of select="@TAMAOHAmountChange"/>
          </div>
          <div class="ui-block-a">
            AOH PercentChange : <xsl:value-of select="@TAMAOHPercentChange"/>
          </div>
          <div class="ui-block-b">
            AOH BaseChange : <xsl:value-of select="@TAMAOHBaseChange"/>
          </div>
          <div class="ui-block-a">
            AOH BasePercentChange : <xsl:value-of select="@TAMAOHBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            CAP Total : <xsl:value-of select="@TAMCAP"/>
          </div>
          <div class="ui-block-b">
            CAP AmountChange : <xsl:value-of select="@TAMCAPAmountChange"/>
          </div>
          <div class="ui-block-a">
            CAP PercentChange : <xsl:value-of select="@TAMCAPPercentChange"/>
          </div>
          <div class="ui-block-b">
            CAP BaseChange : <xsl:value-of select="@TAMCAPBaseChange"/>
          </div>
          <div class="ui-block-a">
            CAP BasePercentChange : <xsl:value-of select="@TAMCAPBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            Total Total : <xsl:value-of select="@TAMTOTAL"/>
          </div>
          <div class="ui-block-b">
            Total AmountChange : <xsl:value-of select="@TAMAmountChange"/>
          </div>
          <div class="ui-block-a">
            Total PercentChange : <xsl:value-of select="@TAMPercentChange"/>
          </div>
          <div class="ui-block-b">
            Total BaseChange : <xsl:value-of select="@TAMBaseChange"/>
          </div>
          <div class="ui-block-a">
            Total BasePercentChange : <xsl:value-of select="@TAMBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
        </div>
      </div>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>

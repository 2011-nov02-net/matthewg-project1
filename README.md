# Project1 - Store App
Matt Goodman's Revature Training Project 1

<h2>Overview</h2>

This project implements a basic web-based store application where users can place orders for products at various store locations.

<h3>Technologies:</h3>

<ul>
  <li>.NET 5</li>
  <li>ASP.NET Core MVC</li>
  <li>C#</li>
  <li>SQL Server</li>
  <li>EntityFramework Core</li>
  <li>Azure App Service</li>
  <li>Azure DevOps</li>
</ul>

<h3>Features:</h3>

<ul>
  <li>Place orders to store locations for customers</li>
  <li>Add a new customer</li>
  <li>Search customers by name</li>
  <li>Display details of an order</li>
  <li>Display all order history of a store location</li>
  <li>Display all order history of a customer</li>
  <li>Client-side validation</li>
  <li>Server-side validation</li>
  <li>Exception handling</li>
  <li>CSRF prevention</li>
  <li>Persistent data; no prices, customers, order history, etc. hardcoded in C#</li>
  <li>Logging of exceptions, EF SQL commands, and other events</li>
</ul>

<h2>Getting Started</h2>

Download/Clone repository
`git clone https://github.com/2011-nov02-net/matthewg-project1`

<h2>Usage</h2>

Home page displays a welcome screen with options to sign in or register as a new customer.

Selecting one of these options will bring you to a page either to sign in with an existing email address or to create a new user account with a name and email address.

Once signed in, options will appear in the navbar to navigate to various resources, including order placement.


To place an order:

Click 'Place Order', which will display a list of store locations to order from.

Once a location is selected, an order form will be generated to select products to purchase from that location.

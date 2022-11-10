import { resolve } from 'inversify-react';
import React from 'react';
import Button from 'react-bootstrap/Button';
import Container from 'react-bootstrap/Container';
import Form from 'react-bootstrap/Form';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';
import { IApiClient } from './services/Interfaces/IApiClient';

class Navigation extends React.Component {
    @resolve("apiClient") private readonly client!: IApiClient;

    async logIn() {
        await this.client.login();
    }
    async logOut() {
        await this.client.logOut();
    }
    render(): React.ReactNode {
        return (
            <Navbar bg="light"  >
                <Container fluid>
                    <Navbar.Brand href="/">Swarm Portal</Navbar.Brand>
                    
                    <Nav className="me-auto">
                        <Nav.Link className="nav-link" href="/">Home</Nav.Link>
                        <NavDropdown title="Admin" id="admin-nav-dropdown">

                        <NavDropdown.Item href="/Manage/Links">Links</NavDropdown.Item>
                        <NavDropdown.Item href="/Manage/Groups">Groups</NavDropdown.Item>
                        <NavDropdown.Item href="/Manage/Roles">Roles</NavDropdown.Item>
                        
                        </NavDropdown>
                    </Nav>
            
                </Container>
                <Form className="container-fluid d-flex justify-content-end">
                {this.client.isLoggedIn && <Button variant='light' disabled >Welcome {this.client.token.given_name}</Button>}
                {this.client.isLoggedIn && <Button variant='outline-success' onClick={() => this.logOut()}>Logout</Button>}
                {!this.client.isLoggedIn && <Button variant="outline-success me-2" onClick={() => this.logIn()} >Login</Button>} 
                </Form>

            </Navbar>
            
        );


    }    
}

export default Navigation;
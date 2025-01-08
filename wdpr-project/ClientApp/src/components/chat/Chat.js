import React, { Component } from 'react';
import * as signalR from '@microsoft/signalr';
import Button from 'react-bootstrap/Button';
import Card from 'react-bootstrap/Card';

export class Chat extends Component {
    static displayName = Chat.name;
    
    constructor(props) {
        super(props);

        this.state = {
            connection: null,
            messages: [],
            date:[],
            currentUser: '',
            message: '',
        };
    }

    componentDidMount() {
        const { selectedUser, currentUser } = this.props;
    
        const newConnection = new signalR.HubConnectionBuilder()
        .withUrl(process.env.REACT_APP_API_URL + '/ChatHub', {
            transport: signalR.HttpTransportType.LongPolling // or signalR.HttpTransportType.ServerSentEvents
        })
        .build();
        this.setState({ connection: newConnection }, () => {
            this.startConnection(selectedUser, currentUser);
        });
    }
    
    

    startConnection = (selectedUser, currentUser) => {
        const { connection } = this.state;
    
        if (connection) {
            connection.start().then(() => {
                connection.on('newMessage', ( message, date) => {
                    console.log(`newMessage:  - ${message} `);
                    this.setState((prevState) => ({
                        messages: [...prevState.messages, { currentUser, message, date }]
                    }));
                });                
            });
        }
    };
    

    sendMessage = async () => {
        const { connection, message } = this.state;
        
        if (connection) {
            await connection.invoke('SendMessage', this.props.selectedUser,this.props.currentUser, message);
            this.setState({ message: '' });
        }
    };
    
    

    handleUserChange = (e) => {
        this.setState({ currentUser: e.target.value });
    };

    handleMessageChange = (e) => {
        this.setState({ message: e.target.value });
    };

    render() {
        const {  message, messages } = this.state;

        return (
            <div className="chat-container">
                <div className="chat-header"></div>
                <div className="chat-box">
                <div className="chat-messages">
                    {messages.map((msg, index) => (
                        <div key={index} className="message">

                            <div role="listitem" tabIndex="0">
                                <Card>
                                    <Card.Body>
                                    <p>Bericht: {msg.message}</p>
                                    <p>Datum: {msg.date}</p>
                                    <p>Datum: {msg.currentUser}</p>
                                    </Card.Body>
                                </Card>
                            </div>
                        </div>
                    ))}
                     
                </div>

                </div>
                <div className="chat-input">
                    <input
                        type="text"
                        placeholder="Enter your message"
                        value={message}
                        onChange={this.handleMessageChange}
                    />
                    <Button variant="success" onClick={this.sendMessage} >Send</Button>
                </div>
            </div>
        );
    }
}
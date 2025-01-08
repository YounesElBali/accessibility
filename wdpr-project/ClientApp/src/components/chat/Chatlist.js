import React, { Component } from 'react';
import axios from 'axios';
import ListGroup from 'react-bootstrap/ListGroup';
import { Chat } from './Chat';
import { jwtDecode } from 'jwt-decode';
import Card from 'react-bootstrap/Card';
import '../chat/Chat';

export class ChatList extends Component {
  constructor(props) {
    super(props);
    this.fetchUsers = this.fetchUsers.bind(this);
    this.fetchChats = this.fetchChats.bind(this);
    this.extractCurrentUser = this.extractCurrentUser.bind(this);
    this.state = {
      users: [],
      chats: [],
      selectedUser: null,
    };
  }

  componentDidMount() {
    this.fetchUsers();
    this.extractCurrentUser();
    this.fetchChats();
  }

  fetchChats = async () => {
    try {
      const currentUserId = this.extractCurrentUser();
  
      const response = await axios.post(process.env.REACT_APP_API_URL +'/chat/all/' +  currentUserId.userId, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`,
        },
      });
  
      const data = response.data;
      this.setState({ chats: data });
    } catch (error) {
      console.error('Error fetching chats:', error.message);
    }
  };
  

  fetchUsers = async () => {
    const currentUser = this.extractCurrentUser();
    console.log(currentUser.role);
    let path = '/chat/expert';
    try {
      if(currentUser.role === 'Business'){
         path = '/chat/business';
      }
      const response = await axios.get(process.env.REACT_APP_API_URL + path, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`,
          Role: currentUser.role
        },
      });
      const data = response.data;
      this.setState({ users: data });
    } catch (error) {
      console.error('Error fetching users:', error.message);
    }
  };

  createChat = async (userToId) => {
    const authToken = localStorage.getItem('token');
    const decodedToken = jwtDecode(authToken);
    try {
      const currentUserId = this.extractCurrentUser();
      const response = await axios.post(process.env.REACT_APP_API_URL +'/chat/create', { userToId , currentUserId: currentUserId.userId}, {
        headers: {
          Authorization: `Bearer ${decodedToken}`
        }
      });
      const chatRoomId = response.data.id;
      const messages = response.data.messages;
      // eslint-disable-next-line
      const chat = response.data.chat;

      this.setState({ chatRoomId: chatRoomId, messages: messages});
      return chatRoomId;
    } catch (error) {
      console.error('Error creating chat room:', error);
    }
  };
  
  handleUserClick = async (userId) => {
    const chatRoomId = await this.createChat(userId);
    this.setState({ selectedUser: userId, chatRoomId });
  };
  

 extractCurrentUser() {
  const authToken = localStorage.getItem('token');
  if (authToken) {
    try {
      const decodedToken = jwtDecode(authToken);

      const currentUserId = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
      const userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      console.log(userRole);
      this.setState({ currentUser: currentUserId});
      return { userId: currentUserId, role: userRole };
    } catch (error) {
      console.error('Error decoding token:', error);
    }
  }
}


  render() {
    const { users, selectedUser , currentUser, chatRoomId, messages, chats} = this.state;

    return (
      <div>
        <h2>Beschikbare gebruikers</h2>
        <ul>
         
        </ul>
        <ListGroup>
        {users.map((user) => (
            <ListGroup.Item
            key={user.id}
            action
            onClick={() => this.handleUserClick(user.id)}
            className="user-list-item"
            >
            {user.userName}
            </ListGroup.Item>
        ))}
        </ListGroup>
        <h3>Bestaande gesprekken</h3>
        <ul>
         
        </ul>
        <ListGroup>
        {chats.map((chat) => (
            <ListGroup.Item
            key={chat.id}
            action
            className="user-list-item"
            >
            {chat.chatRoomId}
            </ListGroup.Item>
        ))}
        </ListGroup>
        {selectedUser && (
          <div>
            <ul>
                {messages && messages.map(message => (
                  <li key={message.id}>
                 <div role="listitem" tabIndex="0">
                  <Card>
                    <Card.Body>
                      <p>Bericht: {message.message}</p>
                      <p>
                      Van gebruiker: <span>{message.username}</span>
                      </p>
                      <p>Datum: {message.date}</p>
                    </Card.Body>
                  </Card>
                </div>
                  </li>
                ))}
            <Chat selectedUser={selectedUser} currentUser={currentUser} chatRoomId={chatRoomId} messages={messages}/>
            </ul>

          </div>
        )}
      </div>
    );
  }
}

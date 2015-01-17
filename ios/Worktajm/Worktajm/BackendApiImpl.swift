//
//  BackendApiImpl.swift
//  worktajm
//
//  Created by Jim Arnell on 2015-01-06.
//  Copyright (c) 2015 Arnell Consulting AB. All rights reserved.
//

import Foundation
import SwiftyJSON
import Alamofire

class BackendApiImpl : BackendApi {
  // let Host = "http://www.worktajm.com"
  let Host:String = "http://192.168.1.3:9000"
  let AuthPath:String = "/auth/local"
  let ListProjects:String = "/api/projects"
  
  var token:String?
  
  init() {
    
  }
   
  func login(username:String, password:String, completionHandler:String -> Void, errorHandler:[LoginResult] -> Void) {
    var errors = [LoginResult]()
    if (username.isEmpty) {
      errors.append(LoginResult.MissingUsername)
    }
    if (password.isEmpty) {
      errors.append(LoginResult.MissingPassword)
    }
    
    if (errors.isEmpty) {
      let parameters = [
        "email": username,
        "password": password
      ]
      Alamofire
        .request(.POST, Host + AuthPath, parameters: parameters, encoding: .JSON)
        .validate(statusCode: 200..<300)
        .validate(contentType: ["application/json"])
        .responseJSON { (_, response, JSON, _) in
          if (JSON == nil) {
            errors.append(.FailedToConnect)
          } else {
            let info = JSON as NSDictionary
            var jsonToken:String? = info.valueForKey("token") as? String
            var message:String? = info.valueForKey("message") as? String
            if (jsonToken == nil) {
              println("Login failed: \(message)")
              errors.append(.InvalidCredentials)
            } else {
              println("Login successful: token\(jsonToken)")
              self.token = jsonToken
            }
          }
          
          // Call error handler if any errors
          if (errors.count > 0) {
            errorHandler(errors)
          } else {
            self.loadProjects(
              {
                ()->[Project] in
                var projects:[Project] = []
                return projects
              },
              {
                Void->Void in
                return
              }
            )
            completionHandler(self.token!)
          }
      }
    } else {
      errorHandler(errors)
    }
  }
  
  func loadProjects(completionHandler:Void->[Project], errorHandler:Void->Void) {
    // Creating an Instance of the Alamofire Manager
    var manager = Manager.sharedInstance

    // Specifying the Headers we need
    manager.session.configuration.HTTPAdditionalHeaders = [
      "Content-Type": "application/json",
      "Authorization": "Bearer \(token!)"
    ]
    
    var url = Host + ListProjects
    Alamofire
      .request(.GET, url, encoding: .JSON)
      .validate(statusCode: 200..<300)
      .validate(contentType: ["application/json"])
      .responseJSON { (_, response, JSON, error) in
        if let results = JSON as? Array<NSDictionary> {
          var o1:NSDictionary = results[0]
          var o2:NSDictionary = results[1]
          var o3:NSDictionary = results[2]
          println(o1.allKeys)
          println(o1.objectForKey("_id"))
          println(o1.objectForKey("name"))
          println(o1.valueForKey("name"))
          var name = o1.valueForKey("name") as String
          println(name)
//          println(o3)
//          println(o1)
        }
    }
  }
}

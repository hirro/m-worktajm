//
//  RootViewController.swift
//  worktajm
//
//  Created by Jim Arnell on 2015-01-05.
//  Copyright (c) 2015 Arnell Consulting AB. All rights reserved.
//

import UIKit
import SwiftyJSON
import Alamofire

class RootViewController: UIViewController, BackendApi {
  
  private var loginViewController: LoginViewController!
  private var worktajmViewController: UITabBarController!
  private var backendApiImpl:BackendApi = BackendApiImpl()
  
  func login(username:String, password:String, completionHandler:String->Void, errorHandler:[LoginResult]->Void) {
    backendApiImpl.login(username, password: password, completionHandler: completionHandler, errorHandler: errorHandler)
  }
  
  func loadProjects(completionHandler:Void->[Project], errorHandler:Void->Void) {
    backendApiImpl.loadProjects(completionHandler, errorHandler: errorHandler)
  }
  
  
  override func viewDidLoad() {
    super.viewDidLoad()
    
    
    // Do any additional setup after loading the view.
    loginViewController = storyboard?.instantiateViewControllerWithIdentifier("Login")
      as LoginViewController
    loginViewController.view.frame = view.frame
    switchViewController(from: nil, to: loginViewController)
  }
  
  override func didReceiveMemoryWarning() {
    super.didReceiveMemoryWarning()
    // Dispose of any resources that can be recreated.
  }
  
  
  func switchViewToWorktajm() {
    if worktajmViewController?.view.superview == nil {
      if worktajmViewController == nil {
        worktajmViewController = storyboard?.instantiateViewControllerWithIdentifier("Worktajm")
          as UITabBarController
      }
    }
    
    switchViewController(from: loginViewController, to: worktajmViewController)
  }
  
  
  private func switchViewController(from fromVC:UIViewController?, to toVC:UIViewController?) {
    if fromVC != nil {
      fromVC!.willMoveToParentViewController(nil)
      fromVC!.view.removeFromSuperview()
      fromVC!.removeFromParentViewController()
    }
    
    if toVC != nil {
      self.addChildViewController(toVC!)
      self.view.insertSubview(toVC!.view, atIndex: 0)
      toVC!.didMoveToParentViewController(self)
    }
  }
  
}

/***************************************************************************** 
* Copyright 2016 Aurora Solutions 
* 
*    http://www.aurorasolutions.io 
* 
* Aurora Solutions is an innovative services and product company at 
* the forefront of the software industry, with processes and practices 
* involving Domain Driven Design(DDD), Agile methodologies to build 
* scalable, secure, reliable and high performance products.
* 
* TradeSharp is a C# based data feed and broker neutral Algorithmic 
* Trading Platform that lets trading firms or individuals automate 
* any rules based trading strategies in stocks, forex and ETFs. 
* TradeSharp allows users to connect to providers like Tradier Brokerage, 
* IQFeed, FXCM, Blackwood, Forexware, Integral, HotSpot, Currenex, 
* Interactive Brokers and more. 
* Key features: Place and Manage Orders, Risk Management, 
* Generate Customized Reports etc 
* 
* Licensed under the Apache License, Version 2.0 (the "License"); 
* you may not use this file except in compliance with the License. 
* You may obtain a copy of the License at 
* 
*    http://www.apache.org/licenses/LICENSE-2.0 
* 
* Unless required by applicable law or agreed to in writing, software 
* distributed under the License is distributed on an "AS IS" BASIS, 
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
* See the License for the specific language governing permissions and 
* limitations under the License. 
*****************************************************************************/


﻿using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Input;
using System.Windows.Controls;

namespace TradeHubGui.Common
{
    /// <summary>
    /// Common UI related helper methods
    /// </summary>
    public static class UIHelper
    {
        #region find child

        public static IEnumerable<T> GetLogicalChildren<T>(this DependencyObject obj) where T : DependencyObject
        {
            foreach (object child in LogicalTreeHelper.GetChildren(obj))
            {
                if (child is T)
                    yield return child as T;

                if (child is DependencyObject)
                {
                    IEnumerable<T> childFound = (child as DependencyObject).GetLogicalChildren<T>();
                    if (childFound != null)
                    {
                        foreach (var item in childFound)
                        {
                            yield return item;
                        }
                    }
                }
            }

            yield return null;
        }

        public static T GetLogicalChildContained<T>(this DependencyObject obj) where T : DependencyObject
        {
            foreach (object child in LogicalTreeHelper.GetChildren(obj))
            {
                if (child is T)
                    return child as T;

                if (child is DependencyObject)
                {
                    T childFound = (child as DependencyObject).GetLogicalChildContained<T>();
                    if (childFound != null)
                        return childFound;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, a null parent is being returned.</returns>
        public static T FindVisualChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindVisualChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;

                        IEnumerable<T> childItem = FindVisualChildren<T>(child);

                        foreach (T grandchild in childItem)
                        {
                            yield return (T)grandchild;
                        }
                    }
                }
            }
        }

        #endregion

        #region find parent

        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">A direct or indirect child of the
        /// queried item.</param>
        /// <returns>The first parent item that matches the submitted
        /// type parameter. If not matching item can be found, a null
        /// reference is being returned.</returns>
        public static T FindVisualParent<T>(DependencyObject child)
          where T : DependencyObject
        {
            ////get parent item
            DependencyObject parentObject = GetParentObject(child);

            ////we've reached the end of the tree
            if (parentObject == null)
            {
                return null;
            }

            ////check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                ////use recursion to proceed with next level
                return FindVisualParent<T>(parentObject);
            }
        }

        public static T FindLogicalParent<T>(DependencyObject child)
          where T : DependencyObject
        {
            ////get parent item
            DependencyObject parentObject = LogicalTreeHelper.GetParent(child);

            ////we've reached the end of the tree
            if (parentObject == null)
            {
                return null;
            }

            ////check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                ////use recursion to proceed with next level
                return FindLogicalParent<T>(parentObject);
            }
        }

        /// <summary>
        /// Tries to locate a given item within the visual tree,
        /// starting with the dependency object at a given position. 
        /// </summary>
        /// <typeparam name="T">The type of the element to be found
        /// on the visual tree of the element at the given location.</typeparam>
        /// <param name="reference">The main element which is used to perform
        /// hit testing.</param>
        /// <param name="point">The position to be evaluated on the origin.</param>
        public static T FindVisualParentFromPoint<T>(UIElement reference, Point point)
          where T : DependencyObject
        {
            DependencyObject element = reference.InputHitTest(point)
                                         as DependencyObject;
            if (element == null)
            {
                return null;
            }
            else if (element is T)
            {
                return (T)element;
            }
            else
            {
                return FindVisualParent<T>(element);
            }
        }

        /// <summary>
        /// This method is an alternative to WPF's
        /// <see cref="VisualTreeHelper.GetParent"/> method, which also
        /// supports content elements. Do note, that for content element,
        /// this method falls back to the logical tree of the element.
        /// </summary>
        /// <param name="child">The item to be processed.</param>
        /// <returns>The submitted item's parent, if available. Otherwise
        /// null.</returns>
        public static DependencyObject GetParentObject(DependencyObject child)
        {
            if (child == null)
            {
                return null;
            }

            ContentElement contentElement = child as ContentElement;

            if (contentElement != null)
            {
                DependencyObject parent = ContentOperations.GetParent(contentElement);
                if (parent != null)
                {
                    return parent;
                }

                FrameworkContentElement fce = contentElement as FrameworkContentElement;
                return fce != null ? fce.Parent : null;
            }

            ////if it's not a ContentElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(child);
        }

        #endregion

        #region update binding sources

        /// <summary>
        /// Recursively processes a given dependency object and all its
        /// children, and updates sources of all objects that use a
        /// binding expression on a given property.
        /// </summary>
        /// <param name="obj">The dependency object that marks a starting
        /// point. This could be a dialog window or a panel control that
        /// hosts bound controls.</param>
        /// <param name="properties">The properties to be updated if
        /// <paramref name="obj"/> or one of its childs provide it along
        /// with a binding expression.</param>
        public static void UpdateBindingSources(DependencyObject obj, params DependencyProperty[] properties)
        {
            foreach (DependencyProperty depProperty in properties)
            {
                ////check whether the submitted object provides a bound property
                ////that matches the property parameters
                BindingExpression be = BindingOperations.GetBindingExpression(obj, depProperty);
                if (be != null)
                {
                    be.UpdateSource();
                }
            }

            int count = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < count; i++)
            {
                ////process child items recursively
                DependencyObject childObject = VisualTreeHelper.GetChild(obj, i);
                UpdateBindingSources(childObject, properties);
            }
        }

        #endregion

        public static IList<DependencyProperty> GetAllDependancyProperties(this DependencyObject obj)
        {
            return obj.GetAttachedProperties(false);
        }

        public static IList<DependencyProperty> GetAttachedProperties(this DependencyObject obj, bool onlyAttachedProperties)
        {
            List<DependencyProperty> attached = new List<DependencyProperty>();

            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(obj,
                new Attribute[] { new PropertyFilterAttribute(PropertyFilterOptions.All) }))
            {
                DependencyPropertyDescriptor dpd =
                    DependencyPropertyDescriptor.FromProperty(pd);

                if (dpd != null && (!onlyAttachedProperties || (onlyAttachedProperties && dpd.IsAttached)))
                {
                    attached.Add(dpd.DependencyProperty);
                }
            }

            return attached;
        }

        public static bool TryGetDependencyObject<T>(RoutedEventArgs e, out T result)
            where T : class
        {
            DependencyObject dep = e.OriginalSource as DependencyObject;

            // iteratively traverse the visual tree
            while ((dep != null) &&
                    !(dep is T))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
            {
                result = null;
                return false;
            }

            result = dep as T;
            return true;
        }

        public static void RemoveLogicalParent(DependencyObject deObject)
        {
            var parent = VisualTreeHelper.GetParent(deObject);

            var parentAsPanel = parent as Panel;
            if (parentAsPanel != null)
            {
                parentAsPanel.Children.Remove((UIElement)deObject);
            }
            var parentAsContentControl = parent as ContentControl;
            if (parentAsContentControl != null)
            {
                parentAsContentControl.Content = null;
            }
            var parentAsDecorator = parent as Decorator;
            if (parentAsDecorator != null)
            {
                parentAsDecorator.Child = null;
            }
        }
    }
}
